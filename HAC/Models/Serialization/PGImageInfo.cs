using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HAC.Models.Serialization
{
    public class PGImageInfo
    {
        public PGImageInfo() {
            Comments = new List<Comment>();          
        }

        public string ImageFileName { get; set; }
        public string ImageFriendlyName { get; set; }
        public string ImageDescription { get; set; }
        public List<Comment> Comments { get; set; }        
    }
}