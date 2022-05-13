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
    public class DoctorController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Doctor
        public ActionResult Index()
        {
            return View(db.Doctori.ToList());
        }

        // GET: Doctor/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.Doctori.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        // GET: Doctor/Create
        public ActionResult Create()
        {
            ViewBag.Specialitati = GetAllSpecialties();
            ViewBag.Clinici = GetAllClinics();
            return View();
        }

        // POST: Doctor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nume,Prenume,DataAngajarii,IdSpecialitate,IdClinica")] DoctorForm doctorForm)
        {
            if (ModelState.IsValid)
            {
                var doctor = new Doctor()
                {
                    Nume = doctorForm.Nume,
                    Prenume = doctorForm.Prenume,
                    DataAngajarii = doctorForm.DataAngajarii
                };

                var specialitate = db.Specialitati.Where(x => x.Id == doctorForm.IdSpecialitate).Select(x => x).ToList();
                doctor.Specialitate = specialitate.FirstOrDefault();

                var clinica = db.Clinici.Where(x => x.Id == doctorForm.IdClinica).Select(x => x).ToList();
                doctor.Clinica = clinica.FirstOrDefault();

                db.Doctori.Add(doctor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(doctorForm);
        }

        // GET: Doctor/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.Doctori.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        // POST: Doctor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nume,Prenume,DataAngajarii,IdSpecialitate,IdClinica")] Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(doctor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(doctor);
        }

        // GET: Doctor/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.Doctori.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        // POST: Doctor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Doctor doctor = db.Doctori.Find(id);
            db.Doctori.Remove(doctor);
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


        [NonAction]
        private IEnumerable<SelectListItem> GetAllSpecialties()
        {
            var selectList = new List<SelectListItem>();

            var specialitati = db.Specialitati.Select(x => x);

            foreach (var specialitate in specialitati)
            {
                selectList.Add(new SelectListItem
                {
                    Value = specialitate.Id.ToString(),
                    Text = specialitate.Denumire.ToString()
                });
            }

            return selectList;
        }

        [NonAction]
        private IEnumerable<SelectListItem> GetAllClinics()
        {
            var selectList = new List<SelectListItem>();

            var clinici = db.Clinici.Select(x => x);

            foreach (var clinica in clinici)
            {
                selectList.Add(new SelectListItem
                {
                    Value = clinica.Id.ToString(),
                    Text = clinica.Nume.ToString()
                });
            }

            return selectList;
        }
    }
}
