using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace HAC.Domain
{
    public class Profile : Entity
    {

        public virtual string Name { get; set; }
        public virtual Member Member { get; set; }
    }
}
