using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HAC.Models.Serialization
{
    public class PGFolderInfo
    {
        public PGFolderInfo() {
            ImagesInfo = new List<PGImageInfo>();
            SortAction = SortAction.Inherit; //Default value
        }
        
        public int Order { get; set; }
        public SortAction SortAction { get; set; }
        public SecurityInfo Security { get; set; }
        public List<PGImageInfo> ImagesInfo { get; set; }
        
    }
}