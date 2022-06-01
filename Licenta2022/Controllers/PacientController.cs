using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Licenta2022.Models;
using Newtonsoft.Json;

namespace Licenta2022.Controllers
{
    public class PacientController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Pacient
        public ActionResult Index()
        {
            var data = db.Pacienti.Include("Adresa").Include("Asigurare").Select(p => new { 
                Id = p.Id,
                Nume = p.Nume,
                Prenume = p.Prenume,
                Adresa = new {  Localitate = p.Adresa.Localitate.Nume,
                                Strada = p.Adresa.Strada,
                                Numar = p.Adresa.Numar } 
            }).ToList();
            
            ViewBag.Data = data;

            return View();
        }

        // GET: Pacient/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pacient pacient = db.Pacienti.Find(id);
            if (pacient == null)
            {
                return HttpNotFound();
            }
            return View(pacient);
        }

        // GET: Pacient/Create
        public ActionResult Create()
        {
            ViewBag.Adrese = GetAllAddresses();
            ViewBag.Asigurari = GetAllAsigurari();
            return View();
        }

        [NonAction]
        private IEnumerable<SelectListItem> GetAllAddresses()
        {
            var selectList = new List<SelectListItem>();

            var adrese = db.Adrese.Select(x => x);

            foreach (var adresa in adrese)
            {
                selectList.Add(new SelectListItem
                {
                    Value = adresa.Id.ToString(),
                    Text = adresa.Strada.ToString()
                });
            }

            return selectList;
        }

        [NonAction]
        private IEnumerable<SelectListItem> GetAllAsigurari()
        {
            var selectList = new List<SelectListItem>();

            var asigurari = db.Asigurari.Select(x => x);

            foreach (var asigurare in asigurari)
            {
                selectList.Add(new SelectListItem
                {
                    Value = asigurare.Id.ToString(),
                    Text = asigurare.Denumire.ToString()
                });
            }

            return selectList;
        }

        // POST: Pacient/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nume,Prenume,CNP,IdAdresa,IdAsigurare")] PacientForm pacientForm)
        {
            if (ModelState.IsValid)
            {
                var pacient = new Pacient()
                {
                    Nume = pacientForm.Nume,
                    Prenume = pacientForm.Prenume,
                    CNP = pacientForm.CNP,
                    PacientXDiagnostics = new List<PacientXDiagnostic>()
                };

                var adresa = db.Adrese.Where(x => x.Id == pacientForm.IdAdresa).Select(x => x).ToList();
                pacient.Adresa = adresa.FirstOrDefault();

                var asigurare = db.Asigurari.Where(x => x.Id == pacientForm.IdAsigurare).Select(x => x).ToList();
                pacient.Asigurare = asigurare.FirstOrDefault();

                db.Pacienti.Add(pacient);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pacientForm);
        }

        // GET: Pacient/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Pacient dbPacient = db.Pacienti.Find(id);
            
            if (dbPacient == null)
            {
                return HttpNotFound();
            }

            var data = db.Pacienti.Where(pacient => pacient.Id == id).Select(pacient => new
            {
                Id = pacient.Id,
                Nume = pacient.Nume,
                Prenume = pacient.Prenume,
                CNP = pacient.CNP,

                IdAdresa = pacient.Adresa.Id,
                IdLocalitate = pacient.Adresa.Localitate.Id,
                IdAsigurare = pacient.Asigurare.Id
            }).FirstOrDefault();

            var adrese = db.Adrese.Select(adresa => new
            {
                value = adresa.Id,
                label = adresa.Strada + " " + adresa.Numar,
                localitateId = adresa.Localitate.Id
            }).ToList();

            var localitati = db.Localitati.Select(localitate => new
            {
                value = localitate.Id,
                label = localitate.Nume
            }).ToList();

            var asigurari = db.Asigurari.Select(asigurare => new
            {
                value = asigurare.Id,
                label = asigurare.Denumire
            });

            ViewBag.Data = data;
            ViewBag.Adrese = adrese;
            ViewBag.Localitati = localitati;
            ViewBag.Asigurari = asigurari;

            return View();
        }

        // POST: Pacient/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,Nume,Prenume,CNP,IdAdresa,IdAsigurare")] PacientEditForm pacientForm)
        {
            if (ModelState.IsValid)
            {
                var pacient = db.Pacienti.Find(pacientForm.Id);

                pacient.Nume = pacientForm.Nume;
                pacient.Prenume = pacientForm.Prenume;
                pacient.CNP = pacientForm.CNP;

                pacient.Adresa = db.Adrese.Where(adresa => adresa.Id == pacientForm.IdAdresa).FirstOrDefault();
                pacient.Asigurare = db.Asigurari.Where(asigurare => asigurare.Id == pacientForm.IdAsigurare).FirstOrDefault();

                db.Entry(pacient).State = EntityState.Modified;

                db.Entry(pacient.Adresa).State = EntityState.Modified;
                db.Entry(pacient.Asigurare).State = EntityState.Modified;

                db.SaveChanges();
            }

            return View();  
        }

        // GET: Pacient/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pacient pacient = db.Pacienti.Find(id);
            if (pacient == null)
            {
                return HttpNotFound();
            }
            return View(pacient);
        }

        // POST: Pacient/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pacient pacient = db.Pacienti.Find(id);
            db.Pacienti.Remove(pacient);
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

        // GET: Pacient/Create
        public ActionResult AddDiagnostic(int? id)
        {
            ViewBag.Diagnostics = GetAllDiagnoses();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pacient pacient = db.Pacienti.Find(id);
            if (pacient == null)
            {
                return HttpNotFound();
            }
            var pacientForm = new PacientAddDiagnosticForm()
            {
                IdPacient = pacient.Id,
                Nume = pacient.Nume,
                Prenume = pacient.Prenume
            };

            return View(pacientForm);
        }

        [NonAction]
        private IEnumerable<SelectListItem> GetAllDiagnoses()
        {
            var selectList = new List<SelectListItem>();

            var diagnostics = db.Diagnostics.Select(x => x);

            foreach (var diagnostic in diagnostics)
            {
                selectList.Add(new SelectListItem
                {
                    Value = diagnostic.Id.ToString(),
                    Text = diagnostic.Denumire.ToString()
                });
            }

            return selectList;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDiagnostic([Bind(Include = "IdPacient,IdDiagnostic")] PacientAddDiagnosticForm pacientForm)
        {
            if (ModelState.IsValid)
            {
                var pacient = db.Pacienti.Find(pacientForm.IdPacient);

                if (pacient.PacientXDiagnostics == null)
                    pacient.PacientXDiagnostics = new List<PacientXDiagnostic>();
                
                var diag = db.Diagnostics.Where(x => x.Id == pacientForm.IdDiagnostic).Select(x => x).ToList();

                var pxd = new PacientXDiagnostic()
                {
                    IdDiagnostic = pacientForm.IdDiagnostic,
                    IdPacient = pacient.Id,
                    Data = DateTime.Now,
                    Pacient = pacient,
                    Diagnostic = diag.FirstOrDefault()
                };
                pacient.PacientXDiagnostics.Add(pxd);

                //db.PacientXDiagnostics.Add(pxd);
                db.Entry(pacient).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pacientForm);
        }
    }
}
