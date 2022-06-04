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
            var data = db.Doctori.Select(doctor => new
            {
                Id = doctor.Id,
                Nume = doctor.Nume,
                Prenume = doctor.Prenume,
                Specializare = doctor.Specialitate.Denumire,
                Clinica = doctor.Clinica.Nume
            });

            ViewBag.Data = data;

            return View();
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

        // GET
        public ActionResult Program(int? id)
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

            var programForm = new DoctorProgramForm()
            {
                IdDoctor = doctor.Id,
            };

            ViewBag.Templates = db.ProgramTemplates.Select(x => x).ToList();
            ViewBag.UsedDates = doctor.DoctorXProgramTemplates.Select(dxt => dxt.Data);

            return View(programForm);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Program([Bind(Include = "IdDoctor,Programe")] DoctorProgramForm programForm)
        {
            if (ModelState.IsValid)
            {
                var doctor = db.Doctori.Find(programForm.IdDoctor);

                foreach (var program in programForm.Programe)
                {
                    var existentProgram = doctor.DoctorXProgramTemplates.Where(dxt => dxt.IdDoctor == doctor.Id).Where(dxt => dxt.Data.CompareTo(program.Data) == 0).FirstOrDefault();

                    if (existentProgram != null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Exista deja un program pentru data aleasa.");
                    }
                }

                foreach (var program in programForm.Programe)
                {
                    var programTemplate = db.ProgramTemplates.Find(program.IdProgramTemplate);

                    var doctorProgramTemplate = new DoctorXProgramTemplate()
                    {
                        Data = program.Data,
                        Config = programTemplate.Config,

                        Doctor = doctor,
                        IdDoctor = programForm.IdDoctor,

                        ProgramTemplate = programTemplate,
                        IdProgramTemplate = program.IdProgramTemplate,
                    };

                    doctor.DoctorXProgramTemplates.Add(doctorProgramTemplate);
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(programForm);
        }
    }
}
