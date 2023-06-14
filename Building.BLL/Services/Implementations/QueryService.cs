using Building.BLL.Services.Interfaces;
using Building.Domain.Entity;
using Building.Domain.Response;
using Building.DAL.Interfaces;
using Building.Domain.Enum;
using AutoMapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Query = Building.Domain.Entity.Query;
using Building.Domain.DTO;

namespace Building.BLL.Services.Implementations
{
    public class QueryService : IQueryService
    {
        private readonly IQueryRepository repository;
        private readonly IMapper mapper;
        public QueryService(IQueryRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public BaseResponse<bool> Create(Query query)
        {
            try
            {

                var result = repository.Create(query);
                if (result != null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = result,
                        StatusCode = Domain.Enum.StatusCode.OK
                    };
                }
                return new BaseResponse<bool>()
                {
                    StatusCode = Domain.Enum.StatusCode.ServerError,
                    Description = "Failed to create query detail"
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    StatusCode = Domain.Enum.StatusCode.ServerError,
                    Description = ex.Message
                };
            }
        }

        public BaseResponse<bool> Delete(int id)
        {
            try
            {

                var result =  repository.Delete(id);
                if (result != null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = result,
                        StatusCode = Domain.Enum.StatusCode.OK
                    };
                }
                return new BaseResponse<bool>()
                {
                    StatusCode = Domain.Enum.StatusCode.ServerError,
                    Description = "Failed to delete query"
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    StatusCode = Domain.Enum.StatusCode.ServerError,
                    Description = ex.Message
                };
            }
        }

        public async Task<BaseResponse<Query>> GetQuery(int id)
        {
            try
            {

                var result = await repository.Get(id);
                if (result != null)
                {
                    return new BaseResponse<Query>()
                    {
                        Data = result,
                        StatusCode = Domain.Enum.StatusCode.OK
                    };
                }
                return new BaseResponse<Query>()
                {
                    StatusCode = Domain.Enum.StatusCode.ServerError,
                    Description = "Not found query"
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<Query>()
                {
                    StatusCode = Domain.Enum.StatusCode.ServerError,
                    Description = ex.Message
                };
            }
        }

        public BaseResponse<IEnumerable<Query>> GetEmployeeQuery(int id)
        {
            try
            {
                
                var queries=  repository.GetAll();
                
                if (queries != null)
                {
                    var res = queries.Where(c => c.ProrabId == id);
                   
                    return new BaseResponse<IEnumerable<Query>>()
                    {
                        Data = res,
                        StatusCode = StatusCode.OK
                    };
                }
                return new BaseResponse<IEnumerable<Query>>()
                {
                    StatusCode = StatusCode.ServerError,
                    Description = "Not found query"
                };
                

            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Query>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.ServerError
                };
            }
            

        }

        public  BaseResponse<IEnumerable<Query>> GetDelivered(int id)
        {
            try
            {

                var result = repository.GetDelivered(id);
                if (result != null)
                {
                    return new BaseResponse<IEnumerable<Query>>()
                    {
                        Data = result,
                        StatusCode = Domain.Enum.StatusCode.OK
                    };
                }
                return new BaseResponse<IEnumerable<Query>>()
                {
                    StatusCode = Domain.Enum.StatusCode.ServerError,
                    Description = "Not found query"
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Query>>()
                {
                    StatusCode = Domain.Enum.StatusCode.ServerError,
                    Description = ex.Message
                };
            }
        }

        public BaseResponse<IEnumerable<Query>> GetMPZ(int id)
        {
            try
            {

                var result =  repository.GetMPZ(id);
                if (result != null)
                {
                    return new BaseResponse<IEnumerable<Query>>()
                    {
                        Data = result,
                        StatusCode = Domain.Enum.StatusCode.OK
                    };
                }
                return new BaseResponse<IEnumerable<Query>>()
                {
                    StatusCode = Domain.Enum.StatusCode.ServerError,
                    Description = "Not found query"
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Query>>()
                {
                    StatusCode = Domain.Enum.StatusCode.ServerError,
                    Description = ex.Message
                };
            }
        }

        public BaseResponse<IEnumerable<Query>> GetBuildingQuery(int id)
        {
            try
            {

                var queries = repository.GetAll();

                if (queries != null)
                {
                    var res = queries.Where(c => c.SiteId == id && c.State=="В работе");

                    return new BaseResponse<IEnumerable<Query>>()
                    {
                        Data = res,
                        StatusCode = StatusCode.OK
                    };
                }
                return new BaseResponse<IEnumerable<Query>>()
                {
                    StatusCode = StatusCode.ServerError,
                    Description = "Not found query"
                };


            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Query>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.ServerError
                };
            }
        }

        public async Task<BaseResponse<bool>> UpdateQuery(Query entity)
        {
            try
            {

                var result = await repository.Update(entity);
                if (result != null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = result,
                        StatusCode = Domain.Enum.StatusCode.OK
                    };
                }
                return new BaseResponse<bool>()
                {
                    StatusCode = Domain.Enum.StatusCode.ServerError,
                    Description = "Failed to update query"
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    StatusCode = Domain.Enum.StatusCode.ServerError,
                    Description = ex.Message
                };
            }
        }

        public BaseResponse<IEnumerable<Query>> GetSnabQuery(int id)
        {
            try
            {

                var queries =  repository.GetAll();

                if (queries != null)
                {
                    var res = queries.Where(c => c.SnabId == id);

                    return new BaseResponse<IEnumerable<Query>>()
                    {
                        Data = res,
                        StatusCode = StatusCode.OK
                    };
                }
                return new BaseResponse<IEnumerable<Query>>()
                {
                    StatusCode = StatusCode.ServerError,
                    Description = "Not found query"
                };


            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Query>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.ServerError
                };
            }
        }

        public BaseResponse<IEnumerable<Query>> GetDeliveredBySnab(int id)
        {
            try
            {

                var result = repository.GetDeliveredBySnab(id);
                if (result != null)
                {
                    return new BaseResponse<IEnumerable<Query>>()
                    {
                        Data = result,
                        StatusCode = Domain.Enum.StatusCode.OK
                    };
                }
                return new BaseResponse<IEnumerable<Query>>()
                {
                    StatusCode = Domain.Enum.StatusCode.ServerError,
                    Description = "Not found query"
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Query>>()
                {
                    StatusCode = Domain.Enum.StatusCode.ServerError,
                    Description = ex.Message
                };
            }
        }

        public BaseResponse<IEnumerable<Query>> GetNoAgreement()
        {
            try
            {

                var result = repository.GetNoAgreement();
                if (result != null)
                {
                    return new BaseResponse<IEnumerable<Query>>()
                    {
                        Data = result,
                        StatusCode = Domain.Enum.StatusCode.OK
                    };
                }
                return new BaseResponse<IEnumerable<Query>>()
                {
                    StatusCode = Domain.Enum.StatusCode.ServerError,
                    Description = "Not found query"
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Query>>()
                {
                    StatusCode = Domain.Enum.StatusCode.ServerError,
                    Description = ex.Message
                };
            }
        }

        public BaseResponse<IEnumerable<Query>> GetMPZBySiteId(int siteId)
        {
            try
            {

                var result = repository.GetMPZbySiteId(siteId);
                if (result != null)
                {
                    return new BaseResponse<IEnumerable<Query>>()
                    {
                        Data = result,
                        StatusCode = Domain.Enum.StatusCode.OK
                    };
                }
                return new BaseResponse<IEnumerable<Query>>()
                {
                    StatusCode = Domain.Enum.StatusCode.ServerError,
                    Description = "Not found query"
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Query>>()
                {
                    StatusCode = Domain.Enum.StatusCode.ServerError,
                    Description = ex.Message
                };
            }
        }
    }
}
