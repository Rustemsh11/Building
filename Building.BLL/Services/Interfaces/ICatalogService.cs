using Building.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Building.BLL.Services.Interfaces
{
    public interface ICatalogService
    {
        Task<BaseResponse<IQueryable<int>>> GetIdByName(string name);
    }
}
