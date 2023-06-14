using Building.DAL.Interfaces;
using Building.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Building.DAL.Repositories
{
    public class QueryDetailsRepository : IQueryDetailsRepository
    {
        private readonly BuildingContext buildingContext;
        public QueryDetailsRepository(BuildingContext buildingContext)
        {
            this.buildingContext = buildingContext;
        }



        public bool Create(QueryDetail entity)
        {
            if(entity!=null)
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


        /// <summary>
        /// get query details
        /// </summary>
        /// <param name="id">id query</param>
        /// <returns></returns>

        public async Task<QueryDetail> Get(int id)
        {
            return  buildingContext.QueryDetails.FirstOrDefault(c => c.QueryId == id);
            
        }

        public async Task<IEnumerable<QueryDetail>> GetDelivered(int prorabId, string queryState)
        {
            return await buildingContext.QueryDetails.Where(c => c.Query.ProrabId == prorabId && c.State == queryState)
                .Include(c=>c.Query).ThenInclude(c=>c.Snab)
                .ToListAsync();
           
        }

        public async Task<IEnumerable<QueryDetail>> GetDeliveredBySnab(int snabId,string queryState)
        {
            return await buildingContext.QueryDetails.Where(c => c.Query.SnabId == snabId && c.State == queryState)
                .Include(c => c.Query).ThenInclude(c => c.Prorab)
                .Include(c => c.Query).ThenInclude(c => c.Site)
                .Include(c => c.Material).ThenInclude(c => c.Catalog)
                .ToListAsync();
        }
        public async Task<IEnumerable<QueryDetail>> GetDeliveredBySiteID(int siteId,string queryState)
        {
            return await buildingContext.QueryDetails.Where(c => c.Query.SiteId == siteId && c.State == queryState)
                .Include(c => c.Query).ThenInclude(c => c.Prorab)
                .Include(c => c.Query).ThenInclude(c => c.Site)
                .Include(c=>c.Query).ThenInclude(c=>c.Snab)
                .Include(c => c.Material).ThenInclude(c => c.Catalog)
                .ToListAsync();
        }

        /// <summary>
        /// Get one detail(concrete name,id...) of query
        /// </summary>
        public async Task<IEnumerable<QueryDetail>> GetQueryDetailsList(int queryDetailId)
        {
            return await buildingContext.QueryDetails.Where(c => c.ID == queryDetailId)
                .Include(c=>c.Material).ThenInclude(c=>c.Catalog)
                .Include(c=>c.Query)
                .ToListAsync();
        }


        /// <summary>
        /// get all info for concrete query
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QueryDetail>> GetQuery(int id)
        {
            return await buildingContext.QueryDetails
                .Where(c => c.QueryId == id)                
                .Include(c => c.Query).ThenInclude(c => c.Snab)
                .Include(c=>c.Material).ThenInclude(c=>c.Catalog).OrderByDescending(c=>c.QueryId)                
                .ToListAsync();
        }

        public async Task<bool> Update(QueryDetail entity)
        {
            if (entity != null)
            {
                buildingContext.Update(entity);
                await buildingContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<QueryDetail> GetDetailOfQuery(int queryDetailId)
        {
            return await buildingContext.QueryDetails.FirstOrDefaultAsync(c => c.ID == queryDetailId);
        }

        public async Task<IEnumerable<QueryDetail>> GetAllMPZ()
        {
            return await buildingContext.QueryDetails.Where(c => c.State == "Подтверждено")
               .Include(c => c.Query).ThenInclude(c => c.Prorab)
               .Include(c => c.Query).ThenInclude(c => c.Site)
               .Include(c => c.Query).ThenInclude(c => c.Snab)
               .Include(c => c.Material).ThenInclude(c => c.Catalog)
               .ToListAsync();
        }

        public async Task<IEnumerable<QueryDetail>> GetDeliveredOrRefuted()
        {
            return await buildingContext.QueryDetails.Where(c=>c.State == "Заявка доставлена" || c.State == "Опровергнут")
                .Include(c => c.Query).ThenInclude(c => c.Prorab)
                .Include(c => c.Query).ThenInclude(c => c.Site)
                .Include(c => c.Query).ThenInclude(c => c.Snab)
                .Include(c => c.Material).ThenInclude(c => c.Catalog)
                .ToListAsync();
        }

        //public Task<IEnumerable<QueryDetail>> GetMPZBySiteID(int id)
        //{
        //    return buildingContext.QueryDetails.Where(c => c.Query.SiteId == id && c.State == "Подтверждено")
        //        .Include(c => c.Query).ThenInclude(c=>c.Snab)
        //        .Include(c => c.)
        //        .Include(c => c.Prorab).ToList();
        //}
    }
}
