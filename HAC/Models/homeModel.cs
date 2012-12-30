using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HAC.Domain;

namespace HAC.Models
{
    public class HomeModel
    {
        public List<Events> latestEvents { get; set; }
        public Member Member { get; set;}

    public HomeModel()
    {
        Member = new Member()
            {
                Email = "ian@com",
                Password = "TEST"
            };
    
    }

    }
}