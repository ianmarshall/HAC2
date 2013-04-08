using HAC.Domain;
using System.Collections.Generic;

namespace HAC.Models
{
    public class PhotosModel
    {

        public List<pic_categories> Categorieses { get; set; }
        public List<pic_images> Images { get; set; }
        public int PicCount { get; set; }
        public int CatCount { get; set; }
        public pic_images CurrentImage { get; set; }
        public pic_categories CurrentCategory { get; set; }
    }
}