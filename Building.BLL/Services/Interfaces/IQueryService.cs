using Building.Domain.Entity;
using Building.Domain.Response;

namespace Building.BLL.Services.Interfaces
{
    public interface IQueryService
    {       
        BaseResponse<IEnumerable<Query>> GetEmployeeQuery(int id);
        BaseResponse<IEnumerable<Query>> GetBuildingQuery(int id);
        BaseResponse<IEnumerable<Query>> GetSnabQuery(int id);
        BaseResponse<IEnumerable<Query>> GetDeliveredBySnab(int id);
        BaseResponse<IEnumerable<Query>> GetNoAgreement();
        BaseResponse<bool> Create(Query query);
        BaseResponse<bool> Delete(int id);
        Task<BaseResponse<Query>> GetQuery(int id);
        BaseResponse<IEnumerable<Query>> GetDelivered(int id);
        BaseResponse<IEnumerable<Query>> GetMPZ(int id);
        Task<BaseResponse<bool>> UpdateQuery(Query entity);
        BaseResponse<IEnumerable<Query>> GetMPZBySiteId(int siteId);
    }
}
