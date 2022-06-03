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
    public class TrimitereController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Trimiteres
        public ActionResult Index(int? id)
        {
            var data = db.Trimiteri.Select(trimitere => new
            {
                Id = trimitere.Id,

                Pacient = new
                {
                    Id = trimitere.Pacient.Id,
                    Nume = trimitere.Pacient.Nume,
                    Prenume = trimitere.Pacient.Prenume
                },

                DataProgramare = trimitere.Programare.Data,

                Specializare = trimitere.Specialitate.Denumire
            });

            if (id != null)
            {
                var pacient = db.Pacienti.Find(id);

                if (pacient == null)

                {
                    return HttpNotFound();
                }

                data = data.Where(trimitere => trimitere.Pacient.Id == id);
            }

            ViewBag.HasId = id != null;
            ViewBag.Data = data;

            return View();
        }

        // GET: Trimiteres/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Trimitere trimitere = db.Trimiteri.Find(id);
            
            if (trimitere == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.HasId = id != null;

            return View(trimitere);
        }

        // GET: Trimiteres/Create
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

            var specialitati = db.Specialitati.Select(specializare => new
            {
                label = specializare.Denumire,
                value = specializare.Id
            });

            var servicii = db.Servicii.Select(serviciu => new
            {
                label = serviciu.Denumire,
                value = serviciu.Id,

                specializareId = serviciu.Specialitate.Id
            });

            ViewBag.Specialitati = specialitati;
            ViewBag.Servicii = servicii;

            ViewBag.IdPacient = programare.Pacient.Id;
            ViewBag.IdProgramare = programare.Id;
   
            return View();
        }

        // POST: Trimiteres/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "Observatii,IdProgramare,IdPacient,IdSpecializare,IdServicii")] TrimitereForm trimitereForm)
        {
            if (ModelState.IsValid)
            {
                var programare = db.Programari.Find(trimitereForm.IdProgramare);

                if (programare.Trimiere != null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Programarea are deja o trimitere.");
                }

                var trimitere = new Trimitere()
                {
                    Observatii = trimitereForm.Observatii
                };
                var pacient = db.Pacienti.Find(trimitereForm.IdPacient);
                trimitere.Pacient = pacient;

                var specialitate = db.Specialitati.Find(trimitereForm.IdSpecializare);
                trimitere.Specialitate = specialitate;

                if (trimitereForm.IdProgramare != 0)
                {
                    trimitere.Programare = programare;
                    trimitere.ProgramareT = null;
                    programare.Trimiere = trimitere;
                }

                trimitere.org = false;
                trimitere.Servicii = new List<Serviciu>();
                for (int i = 0; i < trimitereForm.IdServicii.Count(); i++)
                {
                    var serviciu = db.Servicii.Find(trimitereForm.IdServicii[i]);
                    trimitere.Servicii.Add(serviciu);
                }

                db.Trimiteri.Add(trimitere);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(trimitereForm);
        }

        // GET: Trimiteres/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trimitere trimitere = db.Trimiteri.Find(id);
            if (trimitere == null)
            {
                return HttpNotFound();
            }
            return View(trimitere);
        }

        // POST: Trimiteres/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Observatii")] Trimitere trimitere)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trimitere).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(trimitere);
        }

        // GET: Trimiteres/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trimitere trimitere = db.Trimiteri.Find(id);
            if (trimitere == null)
            {
                return HttpNotFound();
            }
            return View(trimitere);
        }

        // POST: Trimiteres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Trimitere trimitere = db.Trimiteri.Find(id);
            db.Trimiteri.Remove(trimitere);
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
