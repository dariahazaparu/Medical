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
    public class ProgramareController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: Programare
        [Authorize(Roles = "Doctor")]
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
                    Prenume = programare.Pacient.Prenume
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

            ViewBag.Data = data.ToList();
            ViewBag.data.Reverse();
            ViewBag.HasId = id != null;

            return View();
        }

        // GET: Programare/Details/5
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
                    Prenume = programare.Pacient.Prenume,
                },

                Data = programare.Data,

                TrimitereId = programare.Trimitere != null ? programare.Trimitere.Id : -1,

                Doctor = new
                {
                    Nume = programare.Doctor.Nume,
                    Prenume = programare.Doctor.Prenume,
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
                 servicii = data.Servicii.Append(new {
                    Pret = programareDb.Serviciu.Pret,
                    Denumire = programareDb.Serviciu.Denumire
                });
            }

            ViewBag.Data = data;
            ViewBag.Servicii = servicii;

            return View();
        }

        // GET: Programare/Create
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

        // POST: Programare/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
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
                } else
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
            Pacient pacient = db.Pacienti.Find(trimitere.Programare.Pacient.Id);
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
                trimitere.ProgramareParinte = programare;
                programare.TrimitereParinte = trimitere;

                programare.Reteta = null;

                db.Programari.Add(programare);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(form);
        }
    }
}
