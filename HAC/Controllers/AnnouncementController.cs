﻿using HAC.Domain;
using HAC.Domain.Repositories;
using System;
using System.Linq;
using System.Web.Mvc;

namespace HAC.Controllers
{
    public class AnnouncementController : Controller
    {
        //
        // GET: /Announcement/


        public ActionResult Index()
        {
            var announcementRepository = new AnnouncementRepository();
            var activeAnnouncements = announcementRepository.GetActiveAnnouncements().ToList();
            return View(activeAnnouncements);
        }

        //
        // GET: /Announcement/Details/5

        public ActionResult Details(int id)
        {
            var announcement = new Announcement();
            return View("Create", announcement);
        }

        //
        // GET: /Announcement/Create
        //[Authorize]
        //[HttpGet]
        public ActionResult Create()
        {
            return View("Create");
        }


        //[HttpGet]
        //public ActionResult Amend()
        //{
        //    var announcement = new Announcement();
        //    return PartialView("Amend", announcement);
        //}

        [HttpGet]
        public ActionResult Amend(int id)
        {
            var announcementRepository = new AnnouncementRepository();
            var announcement = announcementRepository.GetAnnouncementById(id);
            return PartialView("Amend", announcement);
        }

        //
        // POST: /Announcement/Create
        //[Authorize]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Save(Announcement announcement)
        {
            try
            {

                var announcementRepository = new AnnouncementRepository();
                announcement.UserName = HttpContext.User.Identity.Name;
                if (HttpContext.Session != null)
                    announcement.UserId = HttpContext.Session["UserId"] as int?;

                announcement.CreateDate = DateTime.Now.Date;
                announcement.LastModified = DateTime.Now.Date;
                announcementRepository.Save(announcement);

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Announcement/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Announcement/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Announcement/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Announcement/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
