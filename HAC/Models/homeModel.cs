using HAC.Domain;
using System.Collections.Generic;

namespace HAC.Models
{
    public class HomeModel
    {

        public List<Announcement> latestAnnouncements { get; set; }
        public int activeAnnouncementsCount { get; set; }
        public List<Event> latestEvents { get; set; }
        public List<Event> latestNews { get; set; }
        public Member Member { get; set; }
        public List<pic_images> latestPhotos { get; set; }



    }
}