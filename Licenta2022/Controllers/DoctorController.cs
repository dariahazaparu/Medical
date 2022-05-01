﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Licenta2022.Models;

namespace Licenta2022.Controllers
{
    public class DoctorController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Doctor
        public ActionResult Index()
        {
            var doctori = db.Doctori.Include("Specialitate").Select(x => x);
            ViewBag.Doctori = doctori;
            
            return View();
        }

        //NEW
        public ActionResult New()
        {
            ViewBag.Specialitati = GetAllSpecialties();

            return View();
        }

        [HttpPost]
        public ActionResult New(Doctor doctor)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var specialitate = db.Specialitati.Where(x=> x.Id == doctor.IdSpecialitate).Select(x => x).ToList();
                    doctor.Specialitate = specialitate.FirstOrDefault();
                    db.Doctori.Add(doctor);
                    db.SaveChanges();
                    TempData["message"] = "Un doctor nou a fost adaugat!";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(doctor);
                }
            }
            catch (Exception e)
            {
                return View(doctor);
            }
        }

        [NonAction]
        private IEnumerable<SelectListItem> GetAllSpecialties()
        {
            var selectList = new List<SelectListItem>();

            var specialitati = db.Specialitati.Select(x => x);

            foreach (var specialitate in specialitati)
            {
                selectList.Add(new SelectListItem
                {
                    Value = specialitate.Id.ToString(),
                    Text = specialitate.Denumire.ToString()
                });
            }

            return selectList;
        }

    }
}