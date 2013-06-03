using HAC.Domain;
using System.Collections.Generic;

namespace HAC.Models
{
    public class NewsModel
    {
        public List<Event> NewsItems { get; set; }
        public Event NewsItem { get; set; }
    }
}