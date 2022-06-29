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
    public class LocalitateController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Admin,Receptie,Doctor,Pacient")]
        public ActionResult Index()
        {
            return View(db.Localitati.ToList());
        }

        [Authorize(Roles = "Admin,Receptie,Doctor,Pacient")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Localitate localitate = db.Localitati.Find(id);
            if (localitate == null)
            {
                return HttpNotFound();
            }
            return View(localitate);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,Nume")] Localitate localitate)
        {
            if (ModelState.IsValid)
            {
                db.Localitati.Add(localitate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(localitate);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Localitate localitate = db.Localitati.Find(id);
            if (localitate == null)
            {
                return HttpNotFound();
            }
            return View(localitate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,Nume")] Localitate localitate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(localitate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(localitate);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Localitate localitate = db.Localitati.Find(id);
            if (localitate == null)
            {
                return HttpNotFound();
            }
            return View(localitate);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Localitate localitate = db.Localitati.Find(id);
            db.Localitati.Remove(localitate);
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
