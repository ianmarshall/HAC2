using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Web;
using HAC.Domain;


namespace HAC.Domain.Repositories
{
   
    public class MemberRepository
    {
        private HACEntities context = new HACEntities();

        public void Save(Member member)
        {
            context.Members.Add(member);
            context.SaveChanges();
        }
        public Member GetMember(string email)
        {
            return context.Members.FirstOrDefault(e => e.Email == email);
        }

    }
}
