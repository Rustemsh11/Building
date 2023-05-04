using Building.DAL.Interfaces;
using Building.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Building.DAL.Repositories
{
    public class BuildingSiteRepository : IBuildingSiteRepository
    {
        private readonly BuildingContext buildingContext;
        public BuildingSiteRepository(BuildingContext buildingContext)
        {
            this.buildingContext = buildingContext;
        }

        public async Task<BuildingSite> Get(int? id)
        {
            return await buildingContext.BuildingSites.FirstOrDefaultAsync(c => c.BuildingId == id);
        }

        public async Task<IEnumerable<BuildingSite>> GetAll()
        {
            return await buildingContext.BuildingSites.ToListAsync();
        }

        public int GetIdByName(string siteName)
        {
            return buildingContext.BuildingSites.Where(c=>c.Name==siteName).Select(c => c.BuildingId).First();
        }
    }
}
