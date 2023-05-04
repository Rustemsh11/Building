using Building.Domain.Entity;
using Building.Domain.Response;

namespace Building.BLL.Services.Interfaces
{
    public interface IBuildingSiteService
    {
        Task<BaseResponse<BuildingSite>> GetBuildingInfo(int? id);        
        Task<BaseResponse<IEnumerable<BuildingSite>>> GetAll();
        BaseResponse<int> GetIdByName(string siteName);
    }
}
