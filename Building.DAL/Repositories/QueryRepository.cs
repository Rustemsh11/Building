using Building.DAL.Interfaces;
using Building.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Building.DAL.Repositories
{
    public class QueryRepository : IQueryRepository
    {
        /// <summary>
        /// interface implementation for entity Query
        /// </summary>

        private readonly BuildingContext buildingContext;

        public QueryRepository(BuildingContext buildingContext)
        {
            this.buildingContext = buildingContext;
        }

       

        /// <summary>
        /// create query
        /// </summary>
        /// <param name="entity"> query</param>
        /// <returns></returns>
        public bool Create(Query entity)
        {
            if (entity != null)
            {
                buildingContext.Add(entity);
                buildingContext.SaveChanges();
                return true;

            }
            return false;
        }

        /// <summary>
        /// delete query
        /// </summary>
        /// <param name="entity"> query</param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            var query=  buildingContext.Queries.FirstOrDefault(c=>c.QueryId==id);
            if (query != null)
            {
                buildingContext.Remove(query);
                buildingContext.SaveChanges();
                return true;
            }
            return false;
        }

        
        /// <summary>
        /// get query for concrete user
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns></returns>
        public async Task<Query> Get(int id)
        {
            return await buildingContext.Queries.FirstOrDefaultAsync(x=>x.QueryId==id);
        }


        /// <summary>
        /// get all query
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Query> GetAll()
        {
            var query = buildingContext.Queries
                .Include(c => c.Snab)
                .Include(c=>c.Site)
                .Include(c=>c.Prorab)
                .Include(c=>c.QueryDetails).ThenInclude(c=>c.Material);
            return query;
        }


        /// <summary>
        /// get delivered for concrete prorab query
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<Query> GetDelivered(int id)
        {
            var queries=buildingContext.Queries.Where(c=>c.ProrabId==id && c.State=="Выполнен")
                .Include(c=>c.Snab)
                .ToList();
            return queries;            
        }


        /// <summary>
        /// get delivered by concrete snab query
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<Query> GetDeliveredBySnab(int id)
        {
            return buildingContext.Queries.Where(c => c.SnabId == id && (c.State == "Заявка доставлена" || c.State == "Подтверждено"))                
                .Include(c => c.Site)
                .Include(c => c.Prorab)
                .ToList();
            
        }


        /// <summary>
        /// Get delivered in the site query
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<Query> GetMPZ(int id)
        {
            var queries = buildingContext.Queries.Where(c => c.ProrabId == id && c.State == "Подтверждено")
                .Include(c=>c.Snab)
                .ToList();
            return queries;
        }

        public IEnumerable<Query> GetMPZbySiteId(int siteId)
        {
            return buildingContext.Queries.Where(c => c.SiteId == siteId)
                .Include(c=>c.QueryDetails)
                .Include(c => c.Snab)
                .Include(c => c.Site)
                .Include(c => c.Prorab).ToList();
        }


        /// <summary>
        /// get no agreement query
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Query> GetNoAgreement()
        {
            var queries = buildingContext.Queries.Where(c=>c.Status=="На согласовании")
                .Include(c=>c.Snab)
                .Include(c => c.Prorab)
                .ToList();
            return queries;
        }

        public Task<IQueryable<Query>> GetQuery(int id)
        {
            throw new NotImplementedException();
        }





        /// <summary>
        /// update query
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> Update(Query entity)
        {
            if (entity != null)
            {
                buildingContext.Queries.Update(entity);
                await buildingContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

       
    }
}
