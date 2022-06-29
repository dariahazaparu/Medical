using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Licenta2022.Models;

namespace Licenta2022.Controllers
{
    public class FacturaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Admin, Receptie, Doctor, Pacient")]
        public ActionResult Index(int? id)
        {
            var data = db.Facturi.Select(x => x);

            if (id != null)
            {
                var pacient = db.Pacienti.Find(id);

                if (pacient == null)
                {
                    return HttpNotFound();
                }

                data = data.Where(factura => factura.Programare.Pacient.Id == id);
            }

            ViewBag.HasId = id != null;

            return View(data.ToList());
        }
        public class serv
        {
            public string nume { get; set; }
            public float pret { get; set; }
        };

        [Authorize(Roles = "Admin, Receptie, Doctor, Pacient")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Factura factura = db.Facturi.Find(id);
            if (factura == null)
            {
                return HttpNotFound();
            }

            var servicii = new List<Serviciu>();

            if (factura.Programare.TrimitereParinte != null)
                servicii = factura.Programare.TrimitereParinte.Servicii.ToList();
            if (factura.Programare.Serviciu != null)
                servicii.Add(factura.Programare.Serviciu);
            var abonament = factura.Programare.Pacient.Abonament;
            ViewBag.Servicii = new List<serv>();

            if (abonament != null)
            {
                foreach (var serviciu in servicii)
                {
                    var procent = 0;
                    foreach (var sxa in abonament.ServiciuXAbonamente)
                    {
                        if (sxa.Serviciu == serviciu)
                        {
                            procent = sxa.ProcentReducere;
                            break;
                        }
                    }
                    ViewBag.Servicii.Add(new serv()
                    {
                        nume = serviciu.Denumire,
                        pret = serviciu.Pret * (100 - procent) / 100
                    }); 
                }
            }
            else
            {
                foreach(var serviciu in servicii)
                {
                    ViewBag.Servicii.Add(new serv()
                    {
                        nume = serviciu.Denumire,
                        pret = serviciu.Pret
                    });
                }

            }
            //todo : functie pt calcul pret serviciu din abonament
          

            ViewBag.NumePacient = factura.Programare.Pacient.Nume + " " + factura.Programare.Pacient.Prenume;
            
            return View(factura);
        }

        [Authorize(Roles = "Admin, Doctor")]
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Programare programare = db.Programari.Find(id);
            if (programare == null)
            {
                return HttpNotFound();
            }
            FacturaForm factura = new FacturaForm()
            {
                IdProgramare = programare.Id
            };

            return View(factura);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Doctor")]
        public ActionResult Create([Bind(Include = "IdProgramare")] FacturaForm facturaForm)
        {
            if (ModelState.IsValid)
            {
                Factura factura = new Factura();
                var programare = db.Programari.Find(facturaForm.IdProgramare);
                if (programare.Factura != null)
                {
                    return RedirectToAction("Details", new { id = programare.Factura.Id });
                } 
                factura.Programare = programare;

                var pacient = programare.Pacient;
                var abonament = pacient.Abonament;
                var sxa = abonament != null ? abonament.ServiciuXAbonamente.ToList() : new List<ServiciuXAbonament>();

                factura.Total = 0;

                if (programare.Serviciu == null)
                {
                    foreach (var item in programare.TrimitereParinte.Servicii)
                    {
                        var procent = 0;
                        foreach (var s in sxa)
                        {
                            if (s.Serviciu == item)
                            {
                                procent = s.ProcentReducere;
                                break;
                            }
                        }
                        factura.Total += item.Pret * (100 - procent);
                    }
                }
                else
                {
                    var procent = 0;

                    foreach (var s in sxa)
                    {

                        if (s.Serviciu == programare.Serviciu)
                        {
                            procent = s.ProcentReducere;
                            break;

                        }
                    }
                    factura.Total = programare.Serviciu.Pret * (100 - procent);
                }

                factura.Total /= 100;

                db.Facturi.Add(factura);
                db.SaveChanges();

                return Json(new
                {
                    FacturaId = factura.Id
                });
            }

            return View(facturaForm);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Factura factura = db.Facturi.Find(id);
            if (factura == null)
            {
                return HttpNotFound();
            }
            return View(factura);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Factura factura = db.Facturi.Find(id);
            db.Facturi.Remove(factura);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
