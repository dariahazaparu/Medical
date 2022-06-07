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
                    Id = trimitere.Programare.Pacient.Id,
                    Nume = trimitere.Programare.Pacient.Nume,
                    Prenume = trimitere.Programare.Pacient.Prenume
                },

                DataProgramare = trimitere.Programare.Data,

                Specializare = trimitere.Specializare.Denumire
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

            var trimitere = db.Trimiteri.Where(trim => trim.Id == id);
            
            if (trimitere == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.HasId = id != null;
            ViewBag.Data = trimitere.Select(trim => new
            {
                Id = trim.Id,

                Observatii = trim.Observatii,

                Pacient = new
                {
                    Id = trim.Programare.Pacient.Id,
                    Nume = trim.Programare.Pacient.Nume,
                    Prenume = trim.Programare.Pacient.Prenume
                },

                ProgramareId = trim.Programare.Id,
                ProgramareTId = trim.ProgramareParinte != null ? trim.ProgramareParinte.Id : -1,

                Servicii = trim.Servicii.Select(s => s.Denumire),
                Specializare = trim.Specializare.Denumire
            }).FirstOrDefault();

            return View();
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

                specializareId = serviciu.Specializare.Id
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

                if (programare.Trimitere != null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Programarea are deja o trimitere.");
                }

                var trimitere = new Trimitere()
                {
                    Observatii = trimitereForm.Observatii
                };
                //var pacient = db.Pacienti.Find(trimitereForm.IdPacient);
                //trimitere.Pacient = pacient;

                var Specializare = db.Specialitati.Find(trimitereForm.IdSpecializare);
                trimitere.Specializare = Specializare;

                if (trimitereForm.IdProgramare != 0)
                {
                    trimitere.Programare = programare;
                    trimitere.ProgramareParinte = null;
                    programare.Trimitere = trimitere;
                }

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
