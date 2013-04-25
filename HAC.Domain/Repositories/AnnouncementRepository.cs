using System;
using System.Linq;

namespace HAC.Domain.Repositories
{
    public class AnnouncementRepository
    {
        private HACEntities context = new HACEntities();

        public void Save(Announcement announcement)
        {
            if (announcement.ID == 0)
            {
                context.Announcements.Add(announcement);
            }
            else
            {
                context.Announcements.Attach(announcement);
            }
            context.SaveChanges();
        }

       

        public IQueryable<Announcement> GetLatestAnnouncements(int count)
        {
            return context.Announcements.Where(a => a.ExpiryDate >= DateTime.Now).OrderBy(e => e.ExpiryDate).Take(count);

            //return context.Announcements;
        }


        public IQueryable<Announcement> GetActiveAnnouncements()
        {
            return context.Announcements.Where(a => a.ExpiryDate >= DateTime.Now && a.CreateDate <= DateTime.Now).OrderBy(e => e.ExpiryDate);

        }

        public Announcement GetAnnouncementById(int id)
        {
            return context.Announcements.SingleOrDefault(a => a.ID == id);
        }

    }
}
