using HAC.Domain;
using HAC.Domain.Repositories;
using PagedList;
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

        public object Archive(int? page)
        {
            var announcementRepository = new AnnouncementRepository();
            var archivedAnnouncements = announcementRepository.GetArchivedAnnouncements();
            var pageNumber = page ?? 0; // if no page was specified in the querystring, default to the first page (1)
            var onePageOfAnnouncements = archivedAnnouncements.ToPagedList(pageNumber, 10); // will only contain 25 products max because of the pageSize

            ViewBag.OnePageOfAnnouncements = onePageOfAnnouncements;
            return View("Archive");
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
            Announcement announcement;
            if (id > 0)
            {
                announcement = announcementRepository.GetAnnouncementById(id);
            }
            else
            {
                announcement = new Announcement()
                    {
                        ExpiryDate = System.DateTime.Today
                    };
            }

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

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw (ex);
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
