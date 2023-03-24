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
    public class CatalogService : ICatalogService
    {
        private readonly ICatatalogRepository catatalogRepository;
        public CatalogService(ICatatalogRepository catatalogRepository)
        {
            this.catatalogRepository = catatalogRepository;
        }

        public async Task<BaseResponse<IQueryable<int>>> GetIdByName(string name)
        {
            try
            {
                var id = await catatalogRepository.GetIdByName(name);
                if (id != null)
                {
                    var response = new BaseResponse<IQueryable<int>>()
                    {
                        Data = id,
                        StatusCode = Domain.Enum.StatusCode.OK
                    };
                    return response;
                }
                else
                {
                    var response = new BaseResponse<IQueryable<int>>()
                    {
                        Description = "Employee not found"
                    };
                    return response;
                }

            }
            catch (Exception ex)
            {
                return new BaseResponse<IQueryable<int>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.ServerError
                };
            }
        }
    }
}
