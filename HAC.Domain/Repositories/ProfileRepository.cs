using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Web;
using HAC.Domain;


namespace HAC.Domain.Repositories
{
   
    public class ProfileRepository
    {
        private HACEntities context = new HACEntities();

        public void Save(Profile profile)
        {
            context.Profiles.Add(profile);
            context.SaveChanges();
        }
        public Profile GetProfile(string email)
        {
            return context.Profiles.FirstOrDefault(e => e.Email == email);
        }

    }
}
