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

        // GET: Clinica
        public ActionResult Index()
        {
            return View(db.Clinici.ToList());
        }

        // GET: Clinica/Details/5
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
            return View(clinica);
        }

        // GET: Clinica/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clinica/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nume")] Clinica clinica)
        {
            if (ModelState.IsValid)
            {
                db.Clinici.Add(clinica);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(clinica);
        }

        // GET: Clinica/Edit/5
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

        // POST: Clinica/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // GET: Clinica/Delete/5
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

        // POST: Clinica/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
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
