﻿using System;
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
    public class RetetaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Admin,Receptie,Doctor,Pacient")]
        public ActionResult Index(int? id)
        {
            var data = db.Retete.Select(x => x);

            if (id != null)
            {
                var pacient = db.Pacienti.Find(id);

                if (pacient == null)
                {
                    return HttpNotFound();
                }

                data = data.Where(reteta => reteta.Programare.Pacient.Id == id);
            }

            ViewBag.HasId = id != null;

            return View(data.ToList());
        }

        [Authorize(Roles = "Admin,Receptie,Doctor,Pacient")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reteta reteta = db.Retete.Find(id);
            if (reteta == null)
            {
                return HttpNotFound();
            }

            ViewBag.HasId = id != null;

            return View(reteta);
        }

        [Authorize(Roles = "Admin,Doctor")]
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
            RetetaForm reteta = new RetetaForm()
            {
                IdProgramare = programare.Id
            };

            ViewBag.Medicamente = db.Medicamente.Select(medicament => new
            {
                label = medicament.Denumire,
                value = medicament.Id
            });
            ViewBag.IdProgramare = programare.Id;
            ViewBag.NumePacient = programare.Pacient.Nume + " " + programare.Pacient.Prenume;
            return View();
        }

        [NonAction]
        private IEnumerable<SelectListItem> GetAllMedicine()
        {
            var selectList = new List<SelectListItem>();

            var medicamente = db.Medicamente.Select(x => x).ToList();

            foreach (var medicament in medicamente)
            {
                selectList.Add(new SelectListItem
                {
                    Value = medicament.Id.ToString(),
                    Text = medicament.Denumire.ToString()
                });
            }

            return selectList;
        }

        [Authorize(Roles = "Admin,Doctor")]

        [HttpPost]
        public ActionResult Create([Bind(Include = "IdProgramare,IdMedicamente,Doze")] RetetaForm retetaForm)
        {
            if (ModelState.IsValid)
            {
                var reteta = new Reteta()
                {
                    DataEmiterii = DateTime.Now,
                    RetetaXMedicament = new List<RetetaXMedicament>()
                };

                for (int i = 0; i < retetaForm.IdMedicamente.Count; i++)
                {
                    var idmed = retetaForm.IdMedicamente[i];
                    var doza = retetaForm.Doze[i];
                    var medicament = db.Medicamente.Where(x => x.Id == idmed).Select(x => x).ToList();
                    var rxm = new RetetaXMedicament()
                    {
                        IdMedicament = idmed,
                        IdReteta = reteta.Id,
                        Doza = doza,
                        Reteta = reteta,
                        Medicament = medicament.FirstOrDefault()
                    };
                    reteta.RetetaXMedicament.Add(rxm);
                }

                var programare = db.Programari.Find(retetaForm.IdProgramare);

                programare.Reteta = reteta;

                db.Retete.Add(reteta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(retetaForm);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reteta reteta = db.Retete.Find(id);
            if (reteta == null)
            {
                return HttpNotFound();
            }
            return View(reteta);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Reteta reteta = db.Retete.Find(id);
            db.Retete.Remove(reteta);
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
