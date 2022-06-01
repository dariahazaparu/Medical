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
    public class TrimitereController : Controller
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
        public ActionResult Create(int? id, int? id2)
        {
            if (id == null || id2 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pacient pacient = db.Pacienti.Find(id);
            if (pacient == null)
            {
                return HttpNotFound();
            }
            Programare programare = db.Programari.Find(id2);
            if (programare == null)
            {
                return HttpNotFound();
            }
            var trimitereForm = new TrimitereForm()
            {
                IdPacient = pacient.Id,
                IdProgramare = programare.Id
            };
            ViewBag.Specialitati = GetAllSpecialties();
            ViewBag.Servicii = GetAllServices();
            return View(trimitereForm);
        }


        [NonAction]
        private IEnumerable<Specialitate> GetAllSpecialties()
        {
            var selectList = new List<SelectListItem>();

            var specialitati = db.Specialitati.Select(x => x);

            List<Serviciu> sl = new List<Serviciu>();
            foreach (var specialitate in specialitati)
            {
                selectList.Add(new SelectListItem
                {
                    Value = specialitate.Id.ToString(),
                    Text = specialitate.Denumire.ToString()
                });
            }

            return specialitati;
        }

        [NonAction]
        private IEnumerable<Serviciu> GetAllServices()
        {
            var selectList = new List<SelectListItem>();

            var servicii = db.Servicii.Select(x => x);

            foreach (var serviciu in servicii)
            {
                selectList.Add(new SelectListItem
                {
                    Value = serviciu.Id.ToString(),
                    Text = serviciu.Denumire.ToString()
                });
            }

            return servicii;
        }


        // POST: Trimiteres/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Observatii,IdProgramare,IdPacient,IdSpecializare,IdServicii")] TrimitereForm trimitereForm)
        {
            if (ModelState.IsValid)
            {
                var trimitere = new Trimitere()
                {
                    Observatii = trimitereForm.Observatii
                };
                var pacient = db.Pacienti.Find(trimitereForm.IdPacient);
                trimitere.Pacient = pacient;

                var specialitate = db.Specialitati.Find(trimitereForm.IdSpecializare);
                trimitere.Specialitate = specialitate;

                if (trimitereForm.IdProgramare != 0)
                {
                    var programare = db.Programari.Find(trimitereForm.IdProgramare);
                    trimitere.Programare = programare;
                    trimitere.ProgramareT = null;
                    programare.Trimiere = trimitere;
                }

                trimitere.org = false;
                trimitere.Servicii = new List<Serviciu>();
                for (int i = 0; i< trimitereForm.IdServicii.Count(); i++)
                {
                    var serviciu = db.Servicii.Find(trimitereForm.IdServicii[i]);
                    trimitere.Servicii.Add(serviciu);
                }

                db.Trimiteri.Add(trimitere);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(trimitereForm);
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
