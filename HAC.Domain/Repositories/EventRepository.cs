using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using HAC.Domain;

namespace HAC.Domain.Repositories
{
   
  public class EventRepository
    {
      private HACEntities context = new HACEntities();

        public void Save(Event _event)
        {
            context.Events.Add(_event);
            context.SaveChanges();
        }
        
           

            public IQueryable<Event> GetLatestEvents(int count)
            {
              
                

                return context.Events.Where(e => e.Date >= DateTime.Now).OrderBy(e => e.Date).Take(count);
            }

            public IQueryable<Event> GetLatestNews(int count)
            {
                return context.Events.Where(e => e.Date <= DateTime.Now && e.news1.Length > 1).OrderByDescending(e => e.Date).Take(count);
            }
        
    }
}
