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
    public class ProgramareController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Programare
        public ActionResult Index()
        {
            return View(db.Programari.ToList());
        }

        // GET: Programare/Details/5
        public ActionResult Details(int? id)
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
            return View(programare);
        }

        // GET: Programare/Create
        public ActionResult Create(int? id)
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
            var programareForm = new ProgramareForm()
            {
                IdPacient = pacient.Id
            };
            ViewBag.Doctori = GetAllDoctors();
            return View(programareForm);
        }

        [NonAction]
        private IEnumerable<SelectListItem> GetAllDoctors()
        {
            var selectList = new List<SelectListItem>();

            var doctori = db.Doctori.Select(x => x);

            foreach (var doctor in doctori)
            {
                selectList.Add(new SelectListItem
                {
                    Value = doctor.Id.ToString(),
                    Text = doctor.Nume.ToString() + doctor.Prenume.ToString()
                });
            }

            return selectList;
        }

        // POST: Programare/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Data,IdPacient,IdDoctor")] ProgramareForm programareForm)
        {
            if (ModelState.IsValid)
            {
                var programare = new Programare()
                {
                    Data = programareForm.Data
                };

                var doctor = db.Doctori.Where(x => x.Id == programareForm.IdDoctor).Select(x => x).ToList();
                programare.Doctor = doctor.FirstOrDefault();

                var pacient = db.Pacienti.Where(x => x.Id == programareForm.IdPacient).Select(x => x).ToList();
                programare.Pacient = pacient.FirstOrDefault();

                programare.Retete = new List<Reteta>();
                programare.Trimiere = null;
                programare.TrimitereT = null;

                db.Programari.Add(programare);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(programareForm);
        }

        // GET: Programare/Edit/5
        public ActionResult Edit(int? id)
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
            return View(programare);
        }

        // POST: Programare/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Data")] Programare programare)
        {
            if (ModelState.IsValid)
            {
                db.Entry(programare).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(programare);
        }

        // GET: Programare/Delete/5
        public ActionResult Delete(int? id)
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
            return View(programare);
        }

        // POST: Programare/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Programare programare = db.Programari.Find(id);
            db.Programari.Remove(programare);
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

        public ActionResult CreateFromTrimitere(int? id)
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
            Pacient pacient = db.Pacienti.Find(trimitere.Pacient.Id);
            if (pacient == null)
            {
                return HttpNotFound();
            }
            ProgramareFromForm form = new ProgramareFromForm()
            {
                IdTrimitere = trimitere.Id,
                IdPacient = pacient.Id
            };
            ViewBag.Doctori = GetAllDoctors();
            return View(form);
        }

        // POST: Programare/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFromTrimitere([Bind(Include = "Id,Data,IdPacient,IdDoctor,IdTrimitere")] ProgramareFromForm form)
        {
            if (ModelState.IsValid)
            {
                var programare = new Programare()
                {
                    Data = form.Data
                };

                var doctor = db.Doctori.Where(x => x.Id == form.IdDoctor).Select(x => x).ToList();
                programare.Doctor = doctor.FirstOrDefault();

                var pacient = db.Pacienti.Where(x => x.Id == form.IdPacient).Select(x => x).ToList();
                programare.Pacient = pacient.FirstOrDefault();

                var trimitere = db.Trimiteri.Where(x => x.Id == form.IdTrimitere).Select(x => x).ToList().FirstOrDefault();
                trimitere.org = true;
                trimitere.ProgramareT = programare;
                programare.TrimitereT = trimitere;

                programare.Retete = new List<Reteta>();

                db.Programari.Add(programare);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(form);
        }
    }
}
