using System.Collections.Generic;
using System.Linq;

namespace HAC.Domain.Repositories
{
    public class PhotoRepository
    {
        private HACEntities context = new HACEntities();

        public void Save(pic_images image)
        {
            context.pic_images.Add(image);
            context.SaveChanges();
        }
        public void Save(pic_categories category)
        {
            context.pic_categories.Add(category);
            context.SaveChanges();
        }
        public pic_images GetImage(int id)
        {
            return context.pic_images.FirstOrDefault(p => p.PIC_ID == id);
        }

        public int GetImagesCount()
        {
            return context.pic_images.Count();
        }

        public int GetCategoriesCount()
        {
            return context.pic_categories.Count();
        }

        public List<pic_categories> GetCategories()
        {
            return context.pic_categories.OrderByDescending(c => c.CAT_ID).ToList();
        }

        public List<pic_images> GetCategoryImages(int catId)
        {
            return context.pic_images.Where(i => i.PIC_CAT == catId).ToList();
        }
    }

}
