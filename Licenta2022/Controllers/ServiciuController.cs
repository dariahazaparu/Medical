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
    public class ServiciuController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View(db.Servicii.Include("Specializare").ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Serviciu serviciu = db.Servicii.Find(id);
            if (serviciu == null)
            {
                return HttpNotFound();
            }
            return View(serviciu);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.Specialitati = GetAllSpecialties();
            return View();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,Denumire,Pret,IdSpecializare")] ServiciuForm serviciuForm)
        {
            if (ModelState.IsValid)
            {
                var serviciu = new Serviciu()
                {
                    Denumire = serviciuForm.Denumire,
                    Pret = serviciuForm.Pret
                };

                var Specializare = db.Specialitati.Where(x => x.Id == serviciuForm.IdSpecializare).Select(x => x).ToList();
                serviciu.Specializare = Specializare.FirstOrDefault();

                db.Servicii.Add(serviciu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(serviciuForm);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Serviciu serviciu = db.Servicii.Find(id);
            if (serviciu == null)
            {
                return HttpNotFound();
            }
            return View(serviciu);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,Denumire,IdSpecializare")] Serviciu serviciu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(serviciu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(serviciu);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Serviciu serviciu = db.Servicii.Find(id);
            if (serviciu == null)
            {
                return HttpNotFound();
            }
            return View(serviciu);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Serviciu serviciu = db.Servicii.Find(id);
            db.Servicii.Remove(serviciu);
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
