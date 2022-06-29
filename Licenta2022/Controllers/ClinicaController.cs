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
    public class ClinicaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var data = db.Clinici.Select(clinica => new
            {
                Id = clinica.Id,
                Nume = clinica.Nume,
                Adresa = clinica.Adresa.Localitate.Nume + ", " + clinica.Adresa.Strada + " " + clinica.Adresa.Numar
            });

            ViewBag.Data = data;
            ViewBag.OmitCreate = User.Identity.IsAuthenticated && User.IsInRole("Pacient") || !User.Identity.IsAuthenticated;

            return View();
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clinica clinica = db.Clinici.Find(id);
            if (clinica == null)
            {
                return HttpNotFound();
            }
            ViewBag.Doctori = new List<string>();
            foreach(var doc in clinica.Doctori)
            {
                ViewBag.Doctori.Add(doc.Nume + " " + doc.Prenume + ", " + doc.Specializare.Denumire);
            }
            return View(clinica);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.Adrese = GetAllAddresses();
            ViewBag.Localitati = GetAllCities();
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
                    Text = adresa.Strada.ToString() + ", " + adresa.Numar.ToString()
                });
            }

            return selectList;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,Nume,IdAdresa")] ClinicaForm clinicaForm)
        {
            if (ModelState.IsValid)
            {
                var clinica = new Clinica()
                {
                    Nume = clinicaForm.Nume
                };

                var adresa = db.Adrese.Where(x => x.Id == clinicaForm.IdAdresa).Select(x => x).ToList();
                clinica.Adresa = adresa.FirstOrDefault();

                db.Clinici.Add(clinica);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(clinicaForm);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clinica clinica = db.Clinici.Find(id);
            if (clinica == null)
            {
                return HttpNotFound();
            }
            return View(clinica);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,Nume")] Clinica clinica)
        {
            if (ModelState.IsValid)
            {
                db.Entry(clinica).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(clinica);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clinica clinica = db.Clinici.Find(id);
            if (clinica == null)
            {
                return HttpNotFound();
            }
            return View(clinica);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Clinica clinica = db.Clinici.Find(id);
            db.Clinici.Remove(clinica);
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
