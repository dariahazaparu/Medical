using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Licenta2022.Models;
using Microsoft.AspNet.Identity;

namespace Licenta2022.Controllers
{
    public class DoctorController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var data = db.Doctori.Select(doctor => new
            {
                Id = doctor.Id,
                Nume = doctor.Nume,
                Prenume = doctor.Prenume,
                Specializare = doctor.Specializare.Denumire,
                Clinica = doctor.Clinica.Nume
            });

            ViewBag.Data = data;
            ViewBag.OmitCreate = User.IsInRole("Pacient") || !User.Identity.IsAuthenticated || User.IsInRole("Doctor");

            return View();
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (db.Doctori.Find(id) == null)
            {
                return HttpNotFound();
            }

            var data = db.Doctori.Where(d => d.Id == id).Select(doctor => new
            {
                Id = doctor.Id,
                Nume = doctor.Nume,
                Prenume = doctor.Prenume,
                Specializare = doctor.Specializare.Denumire,
                Clinica = doctor.Clinica.Nume,
                Configuratii = doctor.DoctorXProgramTemplates.Select(dxpt => new
                {
                    Config = dxpt.Config,
                    Data = dxpt.Data
                }).Where(program => program.Data >= DateTime.Today).ToList()
            }).FirstOrDefault();

            ViewBag.Data = data;

            return View();
        }

        [Authorize(Roles = "Admin,Receptie,Doctor")]
        public ActionResult Create()
        {
            ViewBag.Specialitati = GetAllSpecialties();
            ViewBag.Clinici = GetAllClinics();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Receptie,Doctor")]
        public ActionResult Create([Bind(Include = "Id,Nume,Prenume,DataAngajarii,IdSpecializare,IdClinica")] DoctorForm doctorForm)
        {
            if (ModelState.IsValid)
            {
                var doctor = new Doctor()
                {
                    Nume = doctorForm.Nume,
                    Prenume = doctorForm.Prenume,
                    DataAngajarii = doctorForm.DataAngajarii
                };

                var Specializare = db.Specialitati.Where(x => x.Id == doctorForm.IdSpecializare).Select(x => x);
                doctor.Specializare = Specializare.FirstOrDefault();

                var clinica = db.Clinici.Where(x => x.Id == doctorForm.IdClinica).Select(x => x).ToList();
                doctor.Clinica = clinica.FirstOrDefault();

                if (User.Identity.IsAuthenticated && User.IsInRole("Doctor"))
                    doctor.UserId = User.Identity.GetUserId();
                
                db.Doctori.Add(doctor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(doctorForm);
        }

        [Authorize(Roles = "Admin")]
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,Nume,Prenume,DataAngajarii,IdSpecializare,IdClinica")] Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(doctor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(doctor);
        }

        [Authorize(Roles = "Admin,Receptie")]
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Receptie")]
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

            foreach (var Specializare in specialitati)
            {
                selectList.Add(new SelectListItem
                {
                    Value = Specializare.Id.ToString(),
                    Text = Specializare.Denumire.ToString()
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

        [Authorize(Roles = "Admin,Doctor,Receptie")]
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

            ViewBag.Templates = db.ProgramTemplates.Select(x => new
            {
                Id = x.Id,
                Config = x.Config
            }).ToList();
            ViewBag.UsedDates = doctor.DoctorXProgramTemplates.Select(dxt => dxt.Data);
            ViewBag.NumeDoctor = doctor.Nume + " " + doctor.Prenume;
            return View(programForm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Receptie,Doctor")]
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
