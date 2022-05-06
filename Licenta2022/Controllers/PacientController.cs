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
    public class PacientController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Pacient
        public ActionResult Index()
        {
            return View(db.Pacienti.Include("Adresa").Include("Asigurare").ToList());
        }

        // GET: Pacient/Details/5
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
            return View(pacient);
        }

        // GET: Pacient/Create
        public ActionResult Create()
        {
            ViewBag.Adrese = GetAllAddresses();
            ViewBag.Asigurari = GetAllAsigurari();
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
                    Text = adresa.Strada.ToString()
                });
            }

            return selectList;
        }

        [NonAction]
        private IEnumerable<SelectListItem> GetAllAsigurari()
        {
            var selectList = new List<SelectListItem>();

            var asigurari = db.Asigurari.Select(x => x);

            foreach (var asigurare in asigurari)
            {
                selectList.Add(new SelectListItem
                {
                    Value = asigurare.Id.ToString(),
                    Text = asigurare.Denumire.ToString()
                });
            }

            return selectList;
        }

        // POST: Pacient/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nume,Prenume,CNP,IdAdresa,IdAsigurare")] Pacient pacient)
        {
            if (ModelState.IsValid)
            {
                var adresa = db.Adrese.Where(x => x.Id == pacient.IdAdresa).Select(x => x).ToList();
                pacient.Adresa = adresa.FirstOrDefault();

                var asigurare = db.Asigurari.Where(x => x.Id == pacient.IdAsigurare).Select(x => x).ToList();
                pacient.Asigurare = asigurare.FirstOrDefault();

                db.Pacienti.Add(pacient);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pacient);
        }

        // GET: Pacient/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Pacient/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nume,Prenume,CNP,IdAdresa,IdAsigurae")] Pacient pacient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pacient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pacient);
        }

        // GET: Pacient/Delete/5
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

        // POST: Pacient/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
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
    }
}
