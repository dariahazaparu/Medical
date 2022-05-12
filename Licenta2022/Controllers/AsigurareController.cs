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
    public class AsigurareController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Asigurare
        public ActionResult Index()
        {
            return View(db.Asigurari.ToList());
        }

        // GET: Asigurare/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asigurare asigurare = db.Asigurari.Find(id);
            if (asigurare == null)
            {
                return HttpNotFound();
            }
            return View(asigurare);
        }

        // GET: Asigurare/Create
        public ActionResult Create()
        {
            ViewBag.Servicii = GetAllServices();
            return View();
        }

        // POST: Asigurare/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Denumire")] Asigurare asigurare)
        {
            if (ModelState.IsValid)
            {
                db.Asigurari.Add(asigurare);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(asigurare);
        }

        // GET: Asigurare/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asigurare asigurare = db.Asigurari.Find(id);
            if (asigurare == null)
            {
                return HttpNotFound();
            }
            return View(asigurare);
        }

        // POST: Asigurare/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Denumire")] Asigurare asigurare)
        {
            if (ModelState.IsValid)
            {
                db.Entry(asigurare).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(asigurare);
        }

        // GET: Asigurare/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asigurare asigurare = db.Asigurari.Find(id);
            if (asigurare == null)
            {
                return HttpNotFound();
            }
            return View(asigurare);
        }

        // POST: Asigurare/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Asigurare asigurare = db.Asigurari.Find(id);
            db.Asigurari.Remove(asigurare);
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


        // GET: Pacient/Create
        public ActionResult AddService(int? id)
        {
            ViewBag.Servicii = GetAllServices();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asigurare asigurare = db.Asigurari.Find(id);
            if (asigurare == null)
            {
                return HttpNotFound();
            }
            return View(asigurare);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddService([Bind(Include = "Id,Denumire,IdServicii,ProcenteReducere")] Asigurare asigurare)
        {
            if (ModelState.IsValid)
            {
                if (asigurare.ServiciuXAsigurari == null)
                    asigurare.ServiciuXAsigurari = new List<ServiciuXAsigurare>();

                for (int i = 0; i < asigurare.IdServicii.Count(); i++)
                {
                    var currentId = asigurare.IdServicii[i];
                    var currentSale = asigurare.ProcenteReducere[i];
                    var service = db.Servicii.Where(x => x.Id == currentId).Select(x => x).ToList();
                    var sxa = new ServiciuXAsigurare()
                    {
                        IdAsigurare = currentId,
                        IdServiciu = service.FirstOrDefault().Id,
                        ProcentReducere = currentSale,
                        Asigurare = asigurare,
                        Serviciu = service.FirstOrDefault()
                    };

                    asigurare.ServiciuXAsigurari.Add(sxa);
                    service.FirstOrDefault().ServiciuXAsigurari.Add(sxa);
                    db.ServiciuXAsigurari.Add(sxa);
                }

                db.Entry(asigurare).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(asigurare);
        }
    }
}
