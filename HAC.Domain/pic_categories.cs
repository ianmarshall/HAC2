//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HAC.Domain
{
    using System;
    using System.Collections.Generic;
    
    public partial class pic_categories
    {
        public pic_categories()
        {
            this.pic_images = new HashSet<pic_images>();
        }
    
        public int CAT_ID { get; set; }
        public string CAT_NAME { get; set; }
        public string CAT_IMAGE { get; set; }
        public string CAT_DESC { get; set; }
        public int CAT_GROUP { get; set; }
        public Nullable<System.DateTime> CAT_DATED { get; set; }
    
        public virtual ICollection<pic_images> pic_images { get; set; }
        public virtual pic_groups pic_groups { get; set; }
    }
}
