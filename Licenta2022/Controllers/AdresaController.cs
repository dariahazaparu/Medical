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
    public class AdresaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Admin,Receptie,Doctor,Pacient")]
        public ActionResult Index()
        {
            return View(db.Adrese.Include("Localitate").ToList());
        }

        [Authorize(Roles = "Admin,Receptie,Doctor,Pacient")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adresa adresa = db.Adrese.Find(id);
            if (adresa == null)
            {
                return HttpNotFound();
            }
            return View(adresa);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,Strada,Numar,IdLocalitate")] AdresaForm adresaForm)
        {
            if (ModelState.IsValid)
            {
                var adresa = new Adresa()
                {
                    Strada = adresaForm.Strada,
                    Numar = adresaForm.Numar
                };

                var localitate = db.Localitati.Where(x => x.Id == adresaForm.IdLocalitate).Select(x => x).ToList();
                adresa.Localitate = localitate.FirstOrDefault();

                db.Adrese.Add(adresa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(adresaForm);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adresa adresa = db.Adrese.Find(id);
            if (adresa == null)
            {
                return HttpNotFound();
            }
            return View(adresa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,Strada,Numar,IdLocalitate")] Adresa adresa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(adresa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(adresa);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adresa adresa = db.Adrese.Find(id);
            if (adresa == null)
            {
                return HttpNotFound();
            }
            return View(adresa);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Adresa adresa = db.Adrese.Find(id);
            db.Adrese.Remove(adresa);
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
