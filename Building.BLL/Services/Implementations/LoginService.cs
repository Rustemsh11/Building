using AutoMapper;
using Building.BLL.Services.Interfaces;
using Building.DAL.Interfaces;
using Building.DAL.Repositories;
using Building.Domain;
using Building.Domain.DTO;
using Building.Domain.Entity;
using Building.Domain.Enum;
using Building.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Building.BLL.Services.Implementations
{
    public class LoginService:ILoginService
    {
        private readonly IEmployeeRepository  employeeRepository; 
 
        public LoginService(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
           
        }

        private ClaimsIdentity Authenticate(Employee employee)
        {


            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType,employee.Name),
                new Claim(ClaimsIdentity.DefaultRoleClaimType,employee.IdpositionNavigation.Name),
                new Claim(ClaimTypes.Surname,$"{employee.SecondName} {employee.Name}"),
                new Claim(ClaimTypes.NameIdentifier,employee.EmployeeId.ToString())
            };
            return new ClaimsIdentity(claims, "Coockies",
                ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }

        public async Task<BaseResponse<ClaimsIdentity>> Login(LoginViewModel model)
        {
            try
            {
                IQueryable<Employee> getAllUser = await employeeRepository.GetAll();
                var user = getAllUser.FirstOrDefault(c => c.Login == model.Login);
                
                if (user == null)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "User not found"
                    };

                }
                if (model.Password != user.Password)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Password not correct"
                    };

                }
                var result = Authenticate(user);
                
                var response = new BaseResponse<ClaimsIdentity>()
                {
                    Data = result,
                    StatusCode=StatusCode.OK

                };
                return response;
            
            }
            catch(Exception ex)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.ServerError
                };
            }
        }
    }
}
