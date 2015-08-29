using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAccounting.data
{
    class PhotoRepository
    {
        eadbEntities entities = new eadbEntities();

        public Photo getPhoto(int id)
        {
            return entities.Photos.Where(i => i.Id == id).First();
        }

        public void addPhoto(Photo i)
        {
            entities.Photos.Add(i);
            entities.SaveChanges();

        }

        public void deletePhoto(int id)
        {
            Photo i = this.getPhoto(id);
            entities.Photos.Remove(i);
            entities.SaveChanges();
        }

        public void updatePhoto(Photo i)
        {
            entities.Photos.Attach(i);
            entities.Entry(i).State = EntityState.Modified;
            entities.SaveChanges();
        }
    }
}
