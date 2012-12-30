using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HAC.Domain
{
    /// <summary>
    /// HAC Member details
    /// </summary>
    public class Member
    {
        public virtual int Id { get; protected set; }
        public virtual string ForeName { get; set; }
        public virtual string SurName { get; set; }
        public virtual string Email { get; set; }
        public virtual string Phone { get; set; }
        public virtual string Password { get; set; }
        public virtual DateTime DOB { get; set; }
        public virtual string Sex { get; set; }
        public virtual string Access { get; set; }
        public virtual int OldId { get; set; }
        public virtual int MembershipId { get; set; }
        public virtual string MembershipType { get; set; }
        public virtual Profile Profile { get; set; }
    }
}
