using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HAC.Models
{
    public class ViewModelMember
    {
        public int Id { get; protected set; }
        public string ForeName { get; set; }
        public virtual string SurName { get; set; }
        public virtual string Email { get; set; }
        public virtual string Password { get; set; }
        public virtual string Test { get; set; }
        public bool IsAuthenticated { get; set; }

    }
}