using HAC.Domain.Repositories;
using HAC.Models;
using System.Linq;
using System.Web.Mvc;

namespace HAC.Controllers
{
    public class NewsController : Controller
    {
        private EventRepository eventRepository = new EventRepository();

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /News/Details/5

        public ActionResult Details(int id)
        {
            var newsModel = new NewsModel()
                {
                    NewsItems = eventRepository.GetLatestNews(8).ToList(),
                    NewsItem = id > 0 ? eventRepository.GetNewsEvent(id) : eventRepository.GetLatestNews(1).FirstOrDefault()
                };
            return View("Details", newsModel);
        }

        //
        // GET: /News/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /News/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /News/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /News/Edit/5

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
        // GET: /News/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /News/Delete/5

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
