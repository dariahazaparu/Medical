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
    public class MedicamentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Medicament
        public ActionResult Index()
        {
            return View(db.Medicamente.ToList());
        }

        // GET: Medicament/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicament medicament = db.Medicamente.Find(id);
            if (medicament == null)
            {
                return HttpNotFound();
            }
            return View(medicament);
        }

        // GET: Medicament/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Medicament/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Denumire,Observatii")] Medicament medicament)
        {
            if (ModelState.IsValid)
            {
                db.Medicamente.Add(medicament);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(medicament);
        }

        // GET: Medicament/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicament medicament = db.Medicamente.Find(id);
            if (medicament == null)
            {
                return HttpNotFound();
            }
            return View(medicament);
        }

        // POST: Medicament/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Denumire,Observatii")] Medicament medicament)
        {
            if (ModelState.IsValid)
            {
                db.Entry(medicament).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(medicament);
        }

        // GET: Medicament/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicament medicament = db.Medicamente.Find(id);
            if (medicament == null)
            {
                return HttpNotFound();
            }
            return View(medicament);
        }

        // POST: Medicament/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Medicament medicament = db.Medicamente.Find(id);
            db.Medicamente.Remove(medicament);
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
