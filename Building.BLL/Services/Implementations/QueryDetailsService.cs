using Building.BLL.Services.Interfaces;
using Building.DAL.Interfaces;
using Building.Domain.Entity;
using Building.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Building.BLL.Services.Implementations
{
    public class QueryDetailsService : IQueryDetailsService
    {
        private readonly IQueryDetailsRepository queryDetailsRepository;
        public QueryDetailsService(IQueryDetailsRepository queryDetailsRepository)
        {
            this.queryDetailsRepository = queryDetailsRepository;
        }

        public BaseResponse<bool> Create(QueryDetail query)
        {
            try
            {

                var result=queryDetailsRepository.Create(query);
                if (result ==true)
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
            catch(Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    StatusCode = Domain.Enum.StatusCode.ServerError,
                    Description = ex.Message
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<QueryDetail>>> GetAllMPZ()
        {
            try
            {

                var result = await queryDetailsRepository.GetAllMPZ();
                if (result != null)
                {
                    return new BaseResponse<IEnumerable<QueryDetail>>()
                    {
                        Data = result,
                        StatusCode = Domain.Enum.StatusCode.OK
                    };
                }
                return new BaseResponse<IEnumerable<QueryDetail>>()
                {
                    StatusCode = Domain.Enum.StatusCode.ServerError,
                    Description = "Not found query"
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<QueryDetail>>()
                {
                    StatusCode = Domain.Enum.StatusCode.ServerError,
                    Description = ex.Message
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<QueryDetail>>> GetDelivered(int prorabId, string queryState)
        {
            try
            {

                var result = await queryDetailsRepository.GetDelivered(prorabId, queryState);
                if (result != null)
                {
                    return new BaseResponse<IEnumerable<QueryDetail>>()
                    {
                        Data = result,
                        StatusCode = Domain.Enum.StatusCode.OK
                    };
                }
                return new BaseResponse<IEnumerable<QueryDetail>>()
                {
                    StatusCode = Domain.Enum.StatusCode.ServerError,
                    Description = "Not found query"
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<QueryDetail>>()
                {
                    StatusCode = Domain.Enum.StatusCode.ServerError,
                    Description = ex.Message
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<QueryDetail>>> GetDeliveredBySiteID(int siteId, string queryState)
        {
            try
            {

                var result = await queryDetailsRepository.GetDeliveredBySiteID(siteId, queryState);
                if (result != null)
                {
                    return new BaseResponse<IEnumerable<QueryDetail>>()
                    {
                        Data = result,
                        StatusCode = Domain.Enum.StatusCode.OK
                    };
                }
                return new BaseResponse<IEnumerable<QueryDetail>>()
                {
                    StatusCode = Domain.Enum.StatusCode.ServerError,
                    Description = "Not found query"
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<QueryDetail>>()
                {
                    StatusCode = Domain.Enum.StatusCode.ServerError,
                    Description = ex.Message
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<QueryDetail>>> GetDeliveredBySnab(int snabId,string queryState)
        {
            try
            {

                var result = await queryDetailsRepository.GetDeliveredBySnab(snabId,queryState);
                if (result != null)
                {
                    return new BaseResponse<IEnumerable<QueryDetail>>()
                    {
                        Data = result,
                        StatusCode = Domain.Enum.StatusCode.OK
                    };
                }
                return new BaseResponse<IEnumerable<QueryDetail>>()
                {
                    StatusCode = Domain.Enum.StatusCode.ServerError,
                    Description = "Not found query"
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<QueryDetail>>()
                {
                    StatusCode = Domain.Enum.StatusCode.ServerError,
                    Description = ex.Message
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<QueryDetail>>> GetDeliveredOrRefuted()
        {
            try
            {

                var result = await queryDetailsRepository.GetDeliveredOrRefuted();
                if (result != null)
                {
                    return new BaseResponse<IEnumerable<QueryDetail>>()
                    {
                        Data = result,
                        StatusCode = Domain.Enum.StatusCode.OK
                    };
                }
                return new BaseResponse<IEnumerable<QueryDetail>>()
                {
                    StatusCode = Domain.Enum.StatusCode.ServerError,
                    Description = "Not found query"
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<QueryDetail>>()
                {
                    StatusCode = Domain.Enum.StatusCode.ServerError,
                    Description = ex.Message
                };
            }
        }

        public async Task<BaseResponse<QueryDetail>> GetDetailOfQuery(int queryDetailId)
        {
            try
            {

                var result = await queryDetailsRepository.GetDetailOfQuery(queryDetailId);
                if (result != null)
                {
                    return new BaseResponse<QueryDetail>()
                    {
                        Data = result,
                        StatusCode = Domain.Enum.StatusCode.OK
                    };
                }
                return new BaseResponse<QueryDetail>()
                {
                    StatusCode = Domain.Enum.StatusCode.ServerError,
                    Description = "Not found query"
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<QueryDetail>()
                {
                    StatusCode = Domain.Enum.StatusCode.ServerError,
                    Description = ex.Message
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<QueryDetail>>> GetQuery(int id)
        {
            try
            {

                var result = await queryDetailsRepository.GetQuery(id);
                if (result != null)
                {
                    return  new BaseResponse<IEnumerable<QueryDetail>>()
                    {
                        Data = result,
                        StatusCode = Domain.Enum.StatusCode.OK
                    };
                }
                return new BaseResponse<IEnumerable<QueryDetail>>()
                {
                    StatusCode = Domain.Enum.StatusCode.ServerError,
                    Description = "Not found query"
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<QueryDetail>>()
                {
                    StatusCode = Domain.Enum.StatusCode.ServerError,
                    Description = ex.Message
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<QueryDetail>>> GetQueryDetailsList(int queryDetailId)
        {
            try
            {

                var result = await queryDetailsRepository.GetQueryDetailsList(queryDetailId);
                if (result != null)
                {
                    return new BaseResponse<IEnumerable<QueryDetail>>()
                    {
                        Data = result,
                        StatusCode = Domain.Enum.StatusCode.OK
                    };
                }
                return new BaseResponse<IEnumerable<QueryDetail>>()
                {
                    StatusCode = Domain.Enum.StatusCode.ServerError,
                    Description = "Not found query"
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<QueryDetail>>()
                {
                    StatusCode = Domain.Enum.StatusCode.ServerError,
                    Description = ex.Message
                };
            }
        }

        public async Task<BaseResponse<bool>> Update(QueryDetail query)
        {
            try
            {

                var result = await queryDetailsRepository.Update(query);
                if (result == true)
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
        public async Task<BaseResponse<int>> GetNotViewedQueriesForProrab(int prorabId, string queryState)
        {
            try
            {
                var queries = await queryDetailsRepository.GetDelivered(prorabId, queryState);
                int count = queries.Where(c=>c.IsViewed != true).Count();
                return new BaseResponse<int>()
                {
                    Data = count,
                    StatusCode = Domain.Enum.StatusCode.OK
                };


            }
            catch (Exception ex)
            {
                return new BaseResponse<int>()
                {
                    StatusCode = Domain.Enum.StatusCode.ServerError,
                    Description = ex.Message
                };
            }
        }

        public async Task<BaseResponse<bool>> SetViewedQueryDetail(List<QueryDetail> queryDetails)
        {
            try
            {
                foreach (var item in queryDetails)
                {
                    item.IsViewed = true;
                    await queryDetailsRepository.Update(item);
                }
                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = Domain.Enum.StatusCode.OK
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
    }
}
