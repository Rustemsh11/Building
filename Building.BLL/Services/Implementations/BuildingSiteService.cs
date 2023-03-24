using Building.BLL.Services.Interfaces;
using Building.DAL.Interfaces;
using Building.Domain.Entity;
using Building.Domain.Enum;
using Building.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Building.BLL.Services.Implementations
{
    public class BuildingSiteService : IBuildingSiteService
    {
        private readonly IBuildingSiteRepository buildingSiteRepository;
        public BuildingSiteService(IBuildingSiteRepository buildingSiteRepository)
        {
            this.buildingSiteRepository = buildingSiteRepository;
        }

        public async Task<BaseResponse<IEnumerable<BuildingSite>>> GetAll()
        {
            try
            {
                var buildingSite = await buildingSiteRepository.GetAll();
                if (buildingSite != null)
                {
                    var response = new BaseResponse<IEnumerable<BuildingSite>> ()
                    {
                        Data = buildingSite,
                        StatusCode = Domain.Enum.StatusCode.OK
                    };
                    return response;
                }
                else
                {
                    var response = new BaseResponse<IEnumerable<BuildingSite>>()
                    {
                        Description = "Building sites not found"
                    };
                    return response;
                }

            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<BuildingSite>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.ServerError
                };
            }
        }

        public async Task<BaseResponse<BuildingSite>> GetBuildingInfo(int? id)
        {
            try
            {
                var buildingSite = await buildingSiteRepository.Get(id);
                if (buildingSite != null)
                {
                    var response = new BaseResponse<BuildingSite>()
                    {
                        Data = buildingSite,
                        StatusCode = Domain.Enum.StatusCode.OK
                    };
                    return response;
                }
                else
                {
                    var response = new BaseResponse<BuildingSite>()
                    {
                        Description = "Building site not found"
                    };
                    return response;
                }

            }
            catch (Exception ex)
            {
                return new BaseResponse<BuildingSite>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.ServerError
                };
            }
        }
    }
}
