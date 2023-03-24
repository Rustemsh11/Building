using Building.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Building.DAL.Interfaces
{
    public interface IEmployeeRepository:IBaseRepository<Employee>
    {
        Task<IQueryable<Employee>> GetAll();
        Task<EmployeesBuilding> GetEmployeeSite(int id);
        Task<Position> GetEmployeePosition(int positionId);
    }
}
