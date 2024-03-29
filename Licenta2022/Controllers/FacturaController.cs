﻿using System;
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
    public class FacturaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Factura
        public ActionResult Index()
        {
            return View(db.Facturi.ToList());
        }

        // GET: Factura/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Factura factura = db.Facturi.Find(id);
            if (factura == null)
            {
                return HttpNotFound();
            }
            return View(factura);
        }

        // GET: Factura/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Programare programare = db.Programari.Find(id);
            if (programare == null)
            {
                return HttpNotFound();
            }
            FacturaForm factura = new FacturaForm()
            {
                IdProgramare = programare.Id
            };
           
            return View(factura);
        }

        // POST: Factura/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdProgramare")] FacturaForm facturaForm)
        {
            if (ModelState.IsValid)
            {
                Factura factura = new Factura();
                var programare = db.Programari.Find(facturaForm.IdProgramare);
                factura.Programare = programare;

                var pacient = programare.Pacient;
                var asigurare = pacient.Asigurare;
                var sxa = asigurare.ServiciuXAsigurari;

                factura.Total = 0;
                foreach(var item in programare.TrimitereT.Servicii)
                {
                    var procent = 100;
                    foreach(var s in sxa)
                    {
                        if (s.Serviciu == item)
                        {
                            procent = s.ProcentReducere;
                        }
                    }
                    factura.Total += item.Pret * (100 - procent);
                }
                factura.Total /= 100;

                db.Facturi.Add(factura);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(facturaForm);
        }

        // GET: Factura/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Factura factura = db.Facturi.Find(id);
            if (factura == null)
            {
                return HttpNotFound();
            }
            return View(factura);
        }

        // POST: Factura/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Total")] Factura factura)
        {
            if (ModelState.IsValid)
            {
                db.Entry(factura).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(factura);
        }

        // GET: Factura/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Factura factura = db.Facturi.Find(id);
            if (factura == null)
            {
                return HttpNotFound();
            }
            return View(factura);
        }

        // POST: Factura/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Factura factura = db.Facturi.Find(id);
            db.Facturi.Remove(factura);
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
