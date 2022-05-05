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

        // GET: Localitate
        public ActionResult Index()
        {
            return View(db.Localitati.ToList());
        }

        // GET: Localitate/Details/5
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

        // GET: Localitate/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Localitate/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // GET: Localitate/Edit/5
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

        // POST: Localitate/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // GET: Localitate/Delete/5
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

        // POST: Localitate/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
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
