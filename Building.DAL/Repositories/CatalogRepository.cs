using Building.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Building.DAL.Repositories
{
    public class CatalogRepository : ICatatalogRepository
    {
        private readonly BuildingContext buildingContext;
        public CatalogRepository(BuildingContext buildingContext)
        {
            this.buildingContext = buildingContext;
        }

        public async Task<IQueryable<int>> GetIdByName(string name)
        {
            return  buildingContext.Catalogs.Where(c => c.Name == name).Select(c=>c.CatalogID);
        }
    }
}
