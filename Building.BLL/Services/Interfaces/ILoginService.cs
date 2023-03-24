using Building.Domain.Response;
using Building.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Building.Domain.Entity;

namespace Building.BLL.Services.Interfaces
{
    public interface ILoginService
    {
        Task<BaseResponse<ClaimsIdentity>> Login(LoginViewModel model);
    }
}
