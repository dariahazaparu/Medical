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
    public class ProgramareController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Admin,Receptie,Doctor,Pacient")]
        public ActionResult Index(int? id)
        {
            var data = db.Programari.Select(programare => new
            {
                Id = programare.Id,

                Data = programare.Data,

                Doctor = new
                {
                    Nume = programare.Doctor.Nume,
                    Prenume = programare.Doctor.Prenume,
                },

                NumeClinica = programare.Doctor.Clinica.Nume,

                Pacient = new
                {
                    Id = programare.Pacient.Id,
                    Nume = programare.Pacient.Nume,
                    Prenume = programare.Pacient.Prenume,
                    UserId = programare.Pacient.UserId
                },

                Prezent = programare.Prezent
            });

            if (id != null)
            {
                var pacient = db.Pacienti.Find(id);

                if (pacient == null)
                {
                    return HttpNotFound();
                }

                data = data.Where(programare => programare.Pacient.Id == id);
            }

            if (User.IsInRole("Pacient"))
            {
                var userId = User.Identity.GetUserId();
                data = data.Where(prog => prog.Pacient.UserId == userId);
            }

            ViewBag.Data = data.ToList();
            ViewBag.data.Reverse();
            ViewBag.HasId = id != null;

            return View();
        }

        [Authorize(Roles = "Admin,Receptie,Doctor,Pacient")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var programareCursor = db.Programari.Where(prog => prog.Id == id);
            var programareDb = programareCursor.FirstOrDefault();

            if (programareDb == null)
            {
                return HttpNotFound();
            }

            if (User.Identity.GetUserId() != programareDb.Pacient.UserId && User.IsInRole("Pacient"))
                return View("NonAccess");

            var diagnostic = db.PacientXDiagnosticXProgramares.Where(pxd => pxd.IdProgramare == id).FirstOrDefault();

            var diagnosticId = diagnostic == null ? -1 : diagnostic.IdDiagnostic;
            var diagnosticDenumire = diagnostic == null ? "" : diagnostic.Diagnostic.Denumire;

            var adresa = programareDb.Doctor.Clinica.Adresa;

            var adresaClinica = adresa.Localitate.Nume + ", " + adresa.Strada + " " + adresa.Numar;

            var trimitereT = programareDb.TrimitereParinte;

            var trimitereTId = trimitereT != null ? trimitereT.Id : -1;

            var data = programareCursor.Select(programare => new
            {
                Id = programare.Id,
                Prezent = programare.Prezent,

                Pacient = new
                {
                    Nume = programare.Pacient.Nume,
                    Prenume = programare.Pacient.Prenume
                },

                Data = programare.Data,

                TrimitereId = programare.Trimitere != null ? programare.Trimitere.Id : -1,

                Doctor = new
                {
                    Nume = programare.Doctor.Nume,
                    Prenume = programare.Doctor.Prenume
                },

                Clinica = new
                {
                    Nume = programare.Doctor.Clinica.Nume,
                    Adresa = adresaClinica
                },

                Diagnostic = new
                {
                    Id = diagnosticId,
                    Denumire = diagnosticDenumire
                },

                Servicii = programare.TrimitereParinte.Servicii.Select(serviciu => new
                {
                    Pret = serviciu.Pret,
                    Denumire = serviciu.Denumire,
                }),

                TrimitereTId = trimitereTId,

                RetetaId = programare.Reteta != null ? programare.Reteta.Id : -1,

                FacturaId = programare.Factura != null ? programare.Factura.Id : -1
            }).FirstOrDefault();

            var servicii = data.Servicii;

            if (programareDb.Serviciu != null)
            {
                servicii = data.Servicii.Append(new
                {
                    Pret = programareDb.Serviciu.Pret,
                    Denumire = programareDb.Serviciu.Denumire
                });
            }

            ViewBag.Data = data;
            ViewBag.Servicii = servicii;
            ViewBag.IsPacient = User.IsInRole("Pacient");
            return View();
        }

        [Authorize(Roles = "Admin,Receptie,Doctor,Pacient")]
        public ActionResult Create(int? id, int? id2)
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

            if (User.Identity.GetUserId() != pacient.UserId && User.IsInRole("Pacient"))
                return View("NonAccess");

            if (id2 != null)
            {
                var trimitere = db.Trimiteri.Find(id2);

                if (trimitere == null)
                {
                    return HttpNotFound();
                }
                if (trimitere.Programare.Pacient.Id != id)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

            }

            var programareViewSpecialitati = new List<ProgramareViewSpecializare>();

            var specialitati = db.Specialitati.Select(x => x).ToList();

            foreach (var Specializare in specialitati)
            {
                var programareViewSpecializare = new ProgramareViewSpecializare
                {
                    Id = Specializare.Id,
                    Nume = Specializare.Denumire
                };

                var doctoriDb = db.Doctori.Where(doctor => doctor.Specializare.Id == Specializare.Id).ToList();

                var doctori = new List<ProgramareViewDoctor>();

                foreach (var doctorDb in doctoriDb)
                {
                    var programe = new List<ProgramareViewDoctorProgram>();

                    foreach (var program in doctorDb.DoctorXProgramTemplates)
                    {
                        programe.Add(new ProgramareViewDoctorProgram()
                        {
                            Id = program.Id,
                            Config = program.Config,
                            Data = program.Data
                        });
                    }

                    var doctor = new ProgramareViewDoctor()
                    {
                        Id = doctorDb.Id,
                        Nume = doctorDb.Nume,
                        Prenume = doctorDb.Prenume,
                        Programe = programe
                    };

                    doctori.Add(doctor);
                }

                programareViewSpecializare.Doctori = doctori;

                programareViewSpecialitati.Add(programareViewSpecializare);
            }


            ViewBag.Specialitati = programareViewSpecialitati;
            ViewBag.IdPacient = id;
            ViewBag.IdTrimitere = id2 == null ? -1 : id2;
            ViewBag.IdSpecializare = id2 == null ? -1 : db.Trimiteri.Find(id2).Specializare.Id;
            ViewBag.NumePacient = pacient.Nume + " " + pacient.Prenume;

            return View();
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

        [HttpPost]
        [Authorize(Roles = "Admin,Receptie,Doctor")]
        public ActionResult SetPrezent([Bind(Include = "ProgramareId, Prezent")] ProgramareUpdatePrezentForm input)
        {

            var programare = db.Programari.Find(input.ProgramareId);

            if (programare == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Programarea nu exista.");
            }

            programare.Prezent = input.Prezent;

            db.Entry(programare).State = EntityState.Modified;

            db.SaveChanges();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Receptie,Doctor,Pacient")]
        public ActionResult Create([Bind(Include = "IdDoctor,IdProgram,ProgramIntervalIndex,IdPacient,IdTrimitere")] ProgramareCreateForm programareCreateForm)
        {
            if (ModelState.IsValid)
            {
                #region initializari
                var doctor = db.Doctori.Find(programareCreateForm.IdDoctor);

                var pacient = db.Pacienti.Find(programareCreateForm.IdPacient);

                var program = db.DoctorXProgramTemplates.Find(programareCreateForm.IdProgram);

                var trimitereT = db.Trimiteri.Find(programareCreateForm.IdTrimitere);

                #endregion

                var newConfig = program.Config.Substring(0, programareCreateForm.ProgramIntervalIndex) + '0' + program.Config.Substring(programareCreateForm.ProgramIntervalIndex + 1);
                program.Config = newConfig;

                DateTime data = program.Data.AddMinutes(15 * programareCreateForm.ProgramIntervalIndex);
                var programare = new Programare()
                {
                    Doctor = doctor,
                    Pacient = pacient,
                    Data = data,
                    Prezent = false,
                    Trimitere = null,
                    TrimitereParinte = trimitereT,
                    Reteta = null,
                    Serviciu = null
                };

                if (trimitereT != null)
                {
                    trimitereT.ProgramareParinte = programare;
                }
                else
                {
                    programare.Serviciu = db.Servicii
                        .Where(serv => serv.Specializare.Id == doctor.Specializare.Id)
                        .Where(serv => serv.Denumire.Contains("Consultatie"))
                        .FirstOrDefault();
                }

                db.Programari.Add(programare);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(programareCreateForm);
        }

        [Authorize(Roles = "Admin,Receptie,Pacient")]
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

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin,Receptie,Pacient")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Programare programare = db.Programari.Find(id);
            Doctor doctor = programare.Doctor;
            var dt = (int)programare.Data.TimeOfDay.TotalMinutes;
            var config = doctor.DoctorXProgramTemplates.Where(prog => prog.Data.Date == programare.Data.Date).Select(prog => prog.Config).FirstOrDefault();
            config = config.Substring(0, dt / 15) + '1' + config.Substring(dt / 15 + 1);

            doctor.DoctorXProgramTemplates.Where(prog => prog.Data.Date == programare.Data.Date).FirstOrDefault().Config = config;

            var trimitere = programare.TrimitereParinte;
            if (trimitere != null)
            {
                trimitere.ProgramareParinte = null;
            }

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
    }
}
