using HAC.Domain;
using HAC.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace HAC.Controllers
{
    public class EventsController : Controller
    {
        //
        // GET: /Events/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Calendar()
        {
            return View("Calendar");
        }

        [HttpGet]
        public ActionResult Amend(int id)
        {
            var eventRepository = new EventRepository();
            Event _event;

            if (id > 0)
            {
                _event = eventRepository.GetNewsEvent(id);
            }
            else
            {
                _event = new Event();

            }

            return PartialView("Amend", _event);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Save(Event announcement)
        {
            try
            {

                //var announcementRepository = new AnnouncementRepository();
                //announcement.UserName = HttpContext.User.Identity.Name;
                //if (HttpContext.Session != null)
                //    announcement.UserId = HttpContext.Session["UserId"] as int?;

                //announcement.CreateDate = DateTime.Now.Date;
                //announcement.LastModified = DateTime.Now.Date;
                //announcementRepository.Save(announcement);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        public ActionResult CalendarData(double start, double end)
        {
            var fromDate = ConvertFromUnixTimestamp(start);
            var toDate = ConvertFromUnixTimestamp(end);
 
            var eventRepository = new EventRepository();
            IList<CalendarDTO> eventsList = new List<CalendarDTO>();
            foreach (var ev in eventRepository.GetEvents(fromDate, toDate))
            {
                eventsList.Add(new CalendarDTO
                    {
                        id = ev.ID,
                        title = ev.Title,
                        start = ToUnixTimespan(ev.Date.Value),
                        end = ToUnixTimespan(ev.EndDate.Value),
                        url = "www.huntsac.co.uk"
                    });
            }

            return Json(eventsList, JsonRequestBehavior.AllowGet);
        }

        private long ToUnixTimespan(DateTime date)
        {
            TimeSpan tspan = date.ToUniversalTime().Subtract(
         new DateTime(1970, 1, 1, 0, 0, 0));

            return (long)Math.Truncate(tspan.TotalSeconds);
        }


        private static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }
    }
    public class CalendarDTO
    {
        public int id { get; set; }
        public string title { get; set; }
        public long start { get; set; }
        public long end { get; set; }
        public string url { get; set; }
    }
}
