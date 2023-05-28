using Building.Domain.Entity;
using Building.Domain.Response;
using Building.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Building.BLL.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<BaseResponse<Employee>> GetEmployee(int id);
        Task<BaseResponse<EmployeesBuilding>> GetEmployeeSite(int id);
        Task<BaseResponse<ProfileVIewModel>> ProfileInfo(int id);
        Task<BaseResponse<bool>> UpdateEmployee(Employee employee);
        Task<BaseResponse<List<Employee>>> GetAllProrab();

    }
}
