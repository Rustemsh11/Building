using Building.DAL.Interfaces;
using Building.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Building.DAL.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly BuildingContext buildingContext;
        public ImageRepository(BuildingContext buildingContext)
        {
            this.buildingContext = buildingContext;
        }

        public bool Create(Image entity)
        {
            if (entity != null)
            {
                buildingContext.Add(entity);
                buildingContext.SaveChanges();
                return true;

            }
            return false;
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Image> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Image entity)
        {
            throw new NotImplementedException();
        }
    }
}
