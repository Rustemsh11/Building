using AutoMapper;
using Building.BLL.Services.Interfaces;
using Building.DAL.Interfaces;
using Building.DAL.Repositories;
using Building.Domain;
using Building.Domain.DTO;
using Building.Domain.Entity;
using Building.Domain.Enum;
using Building.Domain.Response;
using Building.Domain.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Building.BLL.Services.Implementations
{
    public class LoginService:ILoginService
    {
        private readonly IEmployeeRepository  employeeRepository;
        private readonly IPositionRepository positionRepository;
        private readonly IBuildingSiteRepository buildingSiteRepository;
        private readonly IEmployeeBuildingRepository employeeBuildingRepository;
 
        public LoginService(IEmployeeRepository employeeRepository, IPositionRepository positionRepository, IBuildingSiteRepository buildingSiteRepository, IEmployeeBuildingRepository employeeBuildingRepository)
        {
            this.employeeRepository = employeeRepository;
            this.positionRepository = positionRepository;
            this.buildingSiteRepository = buildingSiteRepository;
            this.employeeBuildingRepository = employeeBuildingRepository;
        }

        public async Task<BaseResponse<List<string>>> GetAllPositionName()
        {
            try
            {
                var positionName = await employeeRepository.GetAllPosition().ToListAsync();
                if (positionName != null)
                {
                    var response = new BaseResponse<List<string>>()
                    {
                        Data = positionName,
                        StatusCode = Domain.Enum.StatusCode.OK
                    };
                    return response;
                }
                else
                {
                    var response = new BaseResponse<List<string>>()
                    {
                        Description = "Positions name not found"
                    };
                    return response;
                }

            }
            catch (Exception ex)
            {
                return new BaseResponse<List<string>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.ServerError
                };
            }

        }
        public async Task Registr(EmployeeViewModel employee)
        {
            try
            {
                var user = await employeeRepository.GetAll().FirstOrDefaultAsync(c => c.Login == employee.Login);

                if (user != null)
                {
                    throw new ArgumentException();
                }


                var newEmp = new Employee();

                newEmp.Name = employee.Name;
                newEmp.SecondName = employee.SecondName;
                newEmp.MiddleName = employee.MiddleName;
                newEmp.Idposition = await positionRepository.GetIdPosition().Where(c => c.Name == employee.Position).Select(c => c.Idposition).FirstAsync();
                newEmp.Birthday = employee.Birthday;
                newEmp.Phone = employee.Phone;
                newEmp.Login = employee.Login;
                newEmp.Password = HashPassword(employee.Password);
                
                employeeRepository.Create(newEmp);

                foreach (var item in employee.Site)
                {
                    var emplBuilding = new EmployeesBuilding();

                    emplBuilding.BuildingId = buildingSiteRepository.GetIdByName(item);
                    emplBuilding.EmployeeId = newEmp.EmployeeId;

                    

                    employeeBuildingRepository.Create(emplBuilding);
                }
                
            }
            catch (Exception ex)
            {
                var exeption = new BaseResponse<Employee>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.ServerError,
                };                
            }


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
                IQueryable<Employee> getAllUser =  employeeRepository.GetAll();
                var user = await getAllUser.FirstOrDefaultAsync(c => c.Login == model.Login);
                
                if (user == null)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "User not found"
                    };

                }
                if (user.Password != HashPassword(model.Password))
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
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashByte = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var hash = BitConverter.ToString(hashByte).Replace("-", "").ToLower();
                return hash;
            }
        }
    }
}
