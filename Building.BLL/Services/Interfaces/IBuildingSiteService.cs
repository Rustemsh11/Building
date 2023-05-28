using Building.Domain.Entity;
using Building.Domain.Response;
using Building.Domain.ViewModel;
using Microsoft.AspNetCore.Http;

namespace Building.BLL.Services.Interfaces
{
    public interface IBuildingSiteService
    {
        Task<BaseResponse<BuildingSite>> GetBuildingInfo(int? id);        
        Task<BaseResponse<IEnumerable<BuildingSite>>> GetAll();
        BaseResponse<int> GetIdByName(string siteName);
        Task<BaseResponse<bool>> AddNewSite(BuildingSiteViewModel siteViewModel, List<IFormFile> postedFiles);
    }
}
