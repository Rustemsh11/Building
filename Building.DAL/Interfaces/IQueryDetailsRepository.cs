using Building.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Building.DAL.Interfaces
{
    public interface IQueryDetailsRepository:IBaseRepository<QueryDetail>
    {
        Task<IEnumerable<QueryDetail>> GetQuery(int id);
        Task<QueryDetail> GetDetailOfQuery(int queryDetailId);
        Task<IEnumerable<QueryDetail>> GetQueryDetailsList(int queryDetailId);
        Task<IEnumerable<QueryDetail>> GetDelivered(int prorabId, string queryState);
        Task<IEnumerable<QueryDetail>> GetDeliveredBySnab(int snabId, string queryState);
        Task<IEnumerable<QueryDetail>> GetDeliveredBySiteID(int siteId, string queryState);
        Task<IEnumerable<QueryDetail>> GetDeliveredOrRefuted();

        Task<IEnumerable<QueryDetail>> GetAllMPZ();
    }
}
