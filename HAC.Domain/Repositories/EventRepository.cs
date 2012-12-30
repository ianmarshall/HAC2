using System.Collections.Generic;
using System.Linq;
using NHibernate;

namespace HAC.Domain.Repositories
{
  public class EventRepository
    {
        public void Save(Events _event)
        {
            using (ISession session = DataConfig.GetSession())
            {
                session.BeginTransaction();
                session.SaveOrUpdate(_event);
                session.Transaction.Commit();
            }
        }
        public List<Events> GetRecentEvents(int numberOfEvents)
        {
            using (ISession session = DataConfig.GetSession())
            {
               //var recentEvents =
               //     session.CreateCriteria<Events>().SetMaxResults(numberOfEvents);
               // recentEvents.List();

                return new List<Events>();
            }
        }
    }
}
