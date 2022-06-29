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

        [Authorize(Roles = "Admin,Receptie,Doctor")]
        public ActionResult Index()
        {
            var Data = db.ProgramTemplates.Select(t => new
            {
                Config = t.Config
            });

            ViewBag.Data = Data;

            return View();
        }

        [Authorize(Roles = "Admin,Receptie,Doctor")]
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

        [Authorize(Roles = "Admin,Receptie")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Receptie")]
        public ActionResult Create([Bind(Include = "Config")] ProgramForm programTemplateForm)
        {
            if (ModelState.IsValid)
            {
                var programeTemplateBd = db.ProgramTemplates.Where(pt => pt.Config.Equals(programTemplateForm.Config)).FirstOrDefault();

                if (programeTemplateBd != null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Aceasta configuratie exista deja.");
                }

                ProgramTemplate programTemplate = new ProgramTemplate()
                {
                    Config = programTemplateForm.Config
                };

                db.ProgramTemplates.Add(programTemplate);
                db.SaveChanges();

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }


        [Authorize(Roles = "Admin")]
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

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
