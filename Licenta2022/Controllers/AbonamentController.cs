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
    public class AbonamentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Admin, Receptie, Doctor, Pacient")]
        public ActionResult Index()
        {
            return View(db.Abonamente.ToList());
        }

        [Authorize(Roles = "Admin, Receptie, Doctor, Pacient")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Abonament abonament = db.Abonamente.Find(id);
            if (abonament == null)
            {
                return HttpNotFound();
            }
            return View(abonament);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.Servicii = db.Servicii.Select(s => new
            {
                id = s.Id,
                nume = s.Denumire
            });

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,Denumire")] Abonament abonament)
        {
            if (ModelState.IsValid)
            {
                db.Abonamente.Add(abonament);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(abonament);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Abonament abonament = db.Abonamente.Find(id);
            if (abonament == null)
            {
                return HttpNotFound();
            }
            return View(abonament);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,Denumire")] Abonament abonament)
        {
            if (ModelState.IsValid)
            {
                db.Entry(abonament).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(abonament);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Abonament abonament = db.Abonamente.Find(id);
            if (abonament == null)
            {
                return HttpNotFound();
            }
            return View(abonament);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Abonament abonament = db.Abonamente.Find(id);
            db.Abonamente.Remove(abonament);
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

        [Authorize(Roles = "Admin")]
        public ActionResult AddService(int? id)
        {
            ViewBag.Servicii = GetAllServices();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Abonament abonament = db.Abonamente.Find(id);
            if (abonament == null)
            {
                return HttpNotFound();
            }
            return View(abonament);
        }

        [NonAction]
        private IEnumerable<SelectListItem> GetAllServices()
        {
            var selectList = new List<SelectListItem>();

            var services = db.Servicii.Select(x => x);

            foreach (var service in services)
            {
                selectList.Add(new SelectListItem
                {
                    Value = service.Id.ToString(),
                    Text = service.Denumire.ToString()
                });
            }

            return selectList;
        }
       
    }
}
