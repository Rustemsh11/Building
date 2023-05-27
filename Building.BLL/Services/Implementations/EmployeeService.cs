using Building.BLL.Services.Interfaces;
using Building.DAL.Interfaces;
using Building.Domain.Entity;
using Building.Domain.Enum;
using Building.Domain.Response;
using Building.Domain.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Building.BLL.Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository employeeRepo;
        public EmployeeService(IEmployeeRepository repo)
        {
            employeeRepo = repo;
        }

       

        public async Task<BaseResponse<Employee>> GetEmployee(int id)
        {
            try
            {
                var employee= await employeeRepo.Get(id);
                if(employee != null)
                {
                    var response = new BaseResponse<Employee>()
                    {
                        Data = employee,
                        StatusCode = Domain.Enum.StatusCode.OK
                    };
                    return response;
                }
                else
                {
                    var response = new BaseResponse<Employee>()
                    {
                        Description = "Employee not found"
                    };
                    return response;
                }

            }
            catch(Exception ex) 
            {
                return new BaseResponse<Employee>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.ServerError
                };
            }
           
        }

        public async Task<BaseResponse<EmployeesBuilding>> GetEmployeeSite(int id)
        {
            try
            {
                var employeesite = await employeeRepo.GetEmployeeSite(id);
                if (employeesite != null)
                {
                    var response = new BaseResponse<EmployeesBuilding>()
                    {
                        Data = employeesite,
                        StatusCode = Domain.Enum.StatusCode.OK
                    };
                    return response;
                }
                else
                {
                    var response = new BaseResponse<EmployeesBuilding>()
                    {
                        Description = "Employee site not found"
                    };
                    return response;
                }

            }
            catch (Exception ex)
            {
                return new BaseResponse<EmployeesBuilding>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.ServerError
                };
            }
        }

        public async Task<BaseResponse<ProfileVIewModel>> ProfileInfo(int id)
        {
            try
            {
                var employee = await employeeRepo.Get(id);
                var employeePosition = await employeeRepo.GetEmployeePosition(employee.Idposition);
                if (employee != null && employeePosition!=null)
                {
                    ProfileVIewModel profileVIew = new ProfileVIewModel()
                    {
                        EmployeeId = employee.EmployeeId,
                        Name = employee.Name,
                        SecondName = employee.SecondName,
                        MiddleName = employee.MiddleName,
                        Birthday = employee.Birthday,
                        Phone = employee.Phone,
                        Photo= employee.Photo,
                        Position = employeePosition.Name
                    };
                    var response = new BaseResponse<ProfileVIewModel>()
                    {
                        Data = profileVIew,
                        StatusCode = Domain.Enum.StatusCode.OK
                    };
                    return response;
                }
                else
                {
                    var response = new BaseResponse<ProfileVIewModel>()
                    {
                        Description = "Employee not found"
                    };
                    return response;
                }

            }
            catch (Exception ex)
            {
                return new BaseResponse<ProfileVIewModel>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.ServerError
                };
            }
        }

        public async Task<BaseResponse<bool>> UpdateEmployee(Employee employee)
        {
            try
            {

                var result = await employeeRepo.Update(employee);
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
                    Description = "Failed to update employee"
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
