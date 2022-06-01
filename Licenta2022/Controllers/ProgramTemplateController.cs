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
    public class ProgramTemplateController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ProgramTemplate
        public ActionResult Index()
        {
            return View(db.ProgramTemplates.ToList());
        }

        // GET: ProgramTemplate/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProgramTemplate programTemplate = db.ProgramTemplates.Find(id);
            if (programTemplate == null)
            {
                return HttpNotFound();
            }
            return View(programTemplate);
        }

        // GET: ProgramTemplate/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProgramTemplate/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Config")] ProgramForm programTemplateForm)
        {
            if (ModelState.IsValid)
            {
                ProgramTemplate programTemplate = new ProgramTemplate()
                {
                    Config = programTemplateForm.Config
                };

                db.ProgramTemplates.Add(programTemplate);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(programTemplateForm);
        }

        // GET: ProgramTemplate/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProgramTemplate programTemplate = db.ProgramTemplates.Find(id);
            if (programTemplate == null)
            {
                return HttpNotFound();
            }
            return View(programTemplate);
        }

        // POST: ProgramTemplate/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Data,Config")] ProgramTemplate programTemplate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(programTemplate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(programTemplate);
        }

        // GET: ProgramTemplate/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProgramTemplate programTemplate = db.ProgramTemplates.Find(id);
            if (programTemplate == null)
            {
                return HttpNotFound();
            }
            return View(programTemplate);
        }

        // POST: ProgramTemplate/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProgramTemplate programTemplate = db.ProgramTemplates.Find(id);
            db.ProgramTemplates.Remove(programTemplate);
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
