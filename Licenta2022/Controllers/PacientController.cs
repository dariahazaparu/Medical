using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Licenta2022.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

namespace Licenta2022.Controllers
{
    public class PacientController : Controller
    {
        private Regex regexCNP = new Regex(@"^\d{1}\d{2}(0[1-9]|1[0-2])(0[1-9]|[12]\d|3[01])(0[1-9]|[1-4]\d| 5[0-2]|99)\d{4}$");
        private ApplicationDbContext db = new ApplicationDbContext();

        [NonAction]
        private bool IsCNPValid(string CNP)
        {
            int bigSum = 0;

            string control = "279146358279";

            if (!this.regexCNP.IsMatch(CNP))
            {
                return false;
            }

            for (int i = 0; i < 12; ++i)
            {
                bigSum += Int32.Parse(CNP[i].ToString()) * Int32.Parse(control[i].ToString());
            }

            int controlDigit = bigSum % 11;

            if (controlDigit == 10)
            {
                controlDigit = 1;
            }

            return controlDigit == Int32.Parse(CNP[12].ToString());
        }

        [Authorize(Roles = "Admin,Receptie,Doctor,Pacient")]
        public ActionResult Index()
        {
            var data = db.Pacienti.Include("Adresa").Include("Abonament").Select(p => new PacientView
            {
                Id = p.Id,
                Nume = p.Nume,
                Prenume = p.Prenume,
                Adresa = new AdresaView
                {
                    Localitate = p.Adresa.Localitate.Nume,
                    Strada = p.Adresa.Strada,
                    Numar = p.Adresa.Numar
                },
                UserId = p.UserId
            });
            if (User.IsInRole("Pacient"))
            {
                var userId = User.Identity.GetUserId();
                data = data.Where(pacient => pacient.UserId == userId);
            }

            ViewBag.Data = data.ToList();
            ViewBag.OmitCreate = User.IsInRole("Pacient") && User.Identity.IsAuthenticated && User.Identity.GetUserId() == data.FirstOrDefault().UserId;

            return View();
        }

        public ActionResult IstoricDiagnostic(int? id)
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

        [Authorize(Roles = "Admin,Receptie,Doctor,Pacient")]
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

            var programari = db.Programari.Where(programare => programare.Pacient.Id == pacient.Id).ToList();
            ViewBag.IsDeletable = (programari.Count() == 0) && (User.IsInRole("Admin") || User.IsInRole("Receptie"));

            if (!User.IsInRole("Pacient") || User.Identity.GetUserId() == pacient.UserId)
            {

                return View(pacient);
            }
            return View("NonAccess");
        }

        [Authorize(Roles = "Admin,Receptie,Pacient")]
        public ActionResult Create()
        {
            ViewBag.Adrese = GetAllAddresses();
            ViewBag.Abonamente = GetAllAbonamente();
            ViewBag.Localitati = GetAllCities();
            return View();
        }

        [NonAction]
        private IEnumerable<SelectListItem> GetAllCities()
        {
            var selectList = new List<SelectListItem>();

            var localitati = db.Localitati.Select(x => x);

            foreach (var localitate in localitati)
            {
                selectList.Add(new SelectListItem
                {
                    Value = localitate.Id.ToString(),
                    Text = localitate.Nume.ToString()
                });
            }

            return selectList;
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
                    Text = adresa.Strada.ToString() + ", " + adresa.Numar.ToString()
                });
            }

            return selectList;
        }

        [NonAction]
        private IEnumerable<SelectListItem> GetAllAbonamente()
        {
            var selectList = new List<SelectListItem>();

            var abonamente = db.Abonamente.Select(x => x);

            foreach (var abonament in abonamente)
            {
                selectList.Add(new SelectListItem
                {
                    Value = abonament.Id.ToString(),
                    Text = abonament.Denumire.ToString()
                });
            }

            return selectList;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Receptie,Pacient")]
        public ActionResult Create([Bind(Include = "Id,Nume,Prenume,CNP,IdAdresa,IdAbonament")] PacientInput pacientForm)
        {
            if (ModelState.IsValid)
            {
                if (!this.IsCNPValid(pacientForm.CNP))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "CNP-ul este invalid.");
                }

                var pacient = new Pacient()
                {
                    Nume = pacientForm.Nume,
                    Prenume = pacientForm.Prenume,
                    CNP = pacientForm.CNP,
                    PacientXDiagnosticXProgramare = new List<PacientXDiagnosticXProgramare>(),
                    UserId = null
                };

                var adresa = db.Adrese.Where(x => x.Id == pacientForm.IdAdresa).Select(x => x).ToList();
                pacient.Adresa = adresa.FirstOrDefault();

                var abonament = db.Abonamente.Where(x => x.Id == pacientForm.IdAbonament).Select(x => x).ToList();
                pacient.Abonament = abonament.FirstOrDefault();

                if (User.Identity.IsAuthenticated && User.IsInRole("Pacient"))
                    pacient.UserId = User.Identity.GetUserId();

                db.Pacienti.Add(pacient);
                db.SaveChanges();
                return RedirectToAction("/");
            }

            return View(pacientForm);
        }

        [Authorize(Roles = "Admin,Receptie")]
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
                IdAbonament = pacient.Abonament != null ? pacient.Abonament.Id : -1
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

            var abonamente = db.Abonamente.Select(abonament => new
            {
                value = abonament.Id,
                label = abonament.Denumire
            });

            ViewBag.Data = data;
            ViewBag.Adrese = adrese;
            ViewBag.Localitati = localitati;
            ViewBag.Abonamente = abonamente;

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Receptie")]
        public ActionResult Edit([Bind(Include = "Id,Nume,Prenume,CNP,IdAdresa,IdAbonament")] PacientEditForm pacientForm)
        {
            if (ModelState.IsValid)
            {
                if (!this.IsCNPValid(pacientForm.CNP))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "CNP-ul este invalid.");
                }

                var pacient = db.Pacienti.Find(pacientForm.Id);

                pacient.Nume = pacientForm.Nume;
                pacient.Prenume = pacientForm.Prenume;
                pacient.CNP = pacientForm.CNP;

                pacient.Adresa = db.Adrese.Where(adresa => adresa.Id == pacientForm.IdAdresa).FirstOrDefault();
                pacient.Abonament = db.Abonamente.Where(abonament => abonament.Id == pacientForm.IdAbonament).FirstOrDefault();

                db.Entry(pacient).State = EntityState.Modified;

                db.Entry(pacient.Adresa).State = EntityState.Modified;
                db.Entry(pacient.Abonament).State = EntityState.Modified;

                db.SaveChanges();
            }

            return View();
        }

        [Authorize(Roles = "Admin,Receptie")]
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Receptie")]
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

        [Authorize(Roles = "Admin,Doctor")]
        public ActionResult AddDiagnostic(int? id)
        {
            ViewBag.Diagnostics = GetAllDiagnoses();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var programare = db.Programari.Find(id);

            if (programare == null)
            {
                return HttpNotFound();
            }

            var pacientForm = new PacientAddDiagnosticForm()
            {
                IdPacient = programare.Pacient.Id,
                IdProgramare = programare.Id,
                Nume = programare.Pacient.Nume,
                Prenume = programare.Pacient.Prenume
            };
            ViewBag.NumePacient = programare.Pacient.Nume + " " + programare.Pacient.Prenume;
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
        [Authorize(Roles = "Admin,Doctor")]
        public ActionResult AddDiagnostic([Bind(Include = "IdPacient,IdDiagnostic,IdProgramare")] PacientAddDiagnosticForm pacientForm)
        {
            if (ModelState.IsValid)
            {
                var pacient = db.Pacienti.Find(pacientForm.IdPacient);
                var programare = db.Programari.Find(pacientForm.IdProgramare);

                if (pacient.PacientXDiagnosticXProgramare == null)
                    pacient.PacientXDiagnosticXProgramare = new List<PacientXDiagnosticXProgramare>();

                var diag = db.Diagnostics.Where(x => x.Id == pacientForm.IdDiagnostic).Select(x => x).ToList();

                var pxd = new PacientXDiagnosticXProgramare()
                {
                    IdDiagnostic = pacientForm.IdDiagnostic,
                    IdPacient = pacient.Id,
                    IdProgramare = programare.Id,

                    Data = DateTime.Now,
                    Pacient = pacient,
                    Diagnostic = diag.FirstOrDefault(),
                    Programare = programare,
                };

                pacient.PacientXDiagnosticXProgramare.Add(pxd);

                db.PacientXDiagnosticXProgramares.Add(pxd);
                db.Entry(pacient).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Details", "Programare", new { id = programare.Id });
            }

            return View(pacientForm);
        }
    }
}
