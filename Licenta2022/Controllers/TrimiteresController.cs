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
    public class TrimiteresController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Trimiteres
        public ActionResult Index()
        {
            return View(db.Trimiteri.ToList());
        }

        // GET: Trimiteres/Details/5
        public ActionResult Details(int? id)
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
            return View(trimitere);
        }

        // GET: Trimiteres/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Trimiteres/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Observatii")] Trimitere trimitere)
        {
            if (ModelState.IsValid)
            {
                db.Trimiteri.Add(trimitere);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(trimitere);
        }

        // GET: Trimiteres/Edit/5
        public ActionResult Edit(int? id)
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
            return View(trimitere);
        }

        // POST: Trimiteres/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Observatii")] Trimitere trimitere)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trimitere).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(trimitere);
        }

        // GET: Trimiteres/Delete/5
        public ActionResult Delete(int? id)
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
            return View(trimitere);
        }

        // POST: Trimiteres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Trimitere trimitere = db.Trimiteri.Find(id);
            db.Trimiteri.Remove(trimitere);
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
