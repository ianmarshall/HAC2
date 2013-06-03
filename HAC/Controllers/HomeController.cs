using HAC.Domain.Repositories;
using HAC.Models;
using System.Linq;
using System.Web.Mvc;

namespace HAC.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ViewResult Index()
        {
            EventRepository eventRepository = new EventRepository();
            PhotoRepository photoRepository = new PhotoRepository();
            AnnouncementRepository announcementRepository = new AnnouncementRepository();

            HomeModel homeModel = new HomeModel()
                {
                    latestAnnouncements = announcementRepository.GetLatestAnnouncements(4).ToList(),
                    activeAnnouncementsCount = announcementRepository.GetActiveAnnouncements().Count(),
                    latestEvents = eventRepository.GetLatestEvents(6).ToList(),
                    latestNews = eventRepository.GetLatestNews(3).ToList(),
                    latestPhotos = photoRepository.GetCategoryImages(38)
                };
            ViewBag.FullUrl = Util.GetApplicationFullUrlWithoutLastSlash();

            return View(homeModel);
        }

        public ViewResult SiteMap()
        {
            return View("SiteMap");
        }

        public ViewResult About()
        {

            return View();
        }



        public ViewResult Sports()
        {
            return View();
        }
        [HttpGet]
        public ViewResult Login()
        {

            return View();
        }

        //[HttpPost]
        //[AutoMap(typeof (Member), typeof (ViewModelMember))]
        //public ActionResult Login(ViewModelMember memberLogin)
        //{
        //    ISession session = MvcApplication.SessionFactory.OpenSession();

        //    Member member = session.Get<Member>(1);
        //    //NHibernateUtil.Initialize(loadedMember.Profile);
        //    return View("MembersArea", member);
        //}


        private string feedUrl = "http://newsrss.bbc.co.uk/";

        private string resource = "rss/sportonline_uk_edition/athletics/rss.xml";
        private string user = "";
        private string password = "";



        //public List<GetRssFeed.Models.Item> GetFeed()
        //{
        //    feedUrl = "http://world-track.org/";
        //    resource = "feed";
        //    return GetRssFeed.GetRssFeed.Get(feedUrl, resource).Channels[0].Items;

        //}
    }
}
