using Building.Domain.Entity;
using Building.Domain.Response;

namespace Building.BLL.Services.Interfaces
{
    public interface IQueryDetailsService
    {
        BaseResponse<bool> Create(QueryDetail query);
        Task<BaseResponse<bool>> Update(QueryDetail query);
        Task<BaseResponse<IEnumerable<QueryDetail>>> GetQuery(int id);
        Task<BaseResponse<IEnumerable<QueryDetail>>> GetDelivered(int prorabId, string queryState);
        Task<BaseResponse<IEnumerable<QueryDetail>>> GetDeliveredBySnab(int snabId,string queryState);
        Task<BaseResponse<IEnumerable<QueryDetail>>> GetDeliveredBySiteID(int siteId,string queryState);
        Task<BaseResponse<QueryDetail>> GetDetailOfQuery(int queryDetailId);
        Task<BaseResponse<IEnumerable<QueryDetail>>> GetQueryDetailsList(int queryDetailId);
        Task<BaseResponse<IEnumerable<QueryDetail>>> GetDeliveredOrRefuted();
        Task<BaseResponse<IEnumerable<QueryDetail>>> GetAllMPZ();
    }
}
