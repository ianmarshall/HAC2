using System.Linq;
using NHibernate;
using NHibernate.Linq;


namespace HAC.Domain.Repositories
{
    public class MemberRepository
    {
        public void Save(Member member)
        {
            using (ISession session = DataConfig.GetSession())
            {
                session.BeginTransaction();
                session.SaveOrUpdate(member);
                session.Transaction.Commit();
            }
        }
        public Member GetMember(string email)
        {
            using (ISession session = DataConfig.GetSession())
            {
                Member member = session.Query<Member>().FirstOrDefault(m => m.Email == email);

                return member;

               
            }
        }

    }
}
