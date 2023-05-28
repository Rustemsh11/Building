﻿using Building.DAL.Interfaces;
using Building.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Building.DAL.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        /// <summary>
        /// interface implementation for entity of Employee
        /// </summary>

        private readonly BuildingContext buildingContext;
        public EmployeeRepository(BuildingContext buildingContext)
        {
            this.buildingContext = buildingContext;
        }

        public bool Create(Employee entity)
        {
            if (entity != null)
            {
                buildingContext.Add(entity);
                buildingContext.SaveChanges();
                return true;

            }
            return false;
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Employee> Get(int id)
        {
            return await buildingContext.Employees.FirstOrDefaultAsync(c => c.EmployeeId == id);
        }

        public async Task<bool> Update(Employee entity)
        {
            if (entity != null)
            {
                buildingContext.Employees.Update(entity);
                await buildingContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public IQueryable<Employee> GetAll()
        {
            var employee = buildingContext.Employees.Include(opt => opt.IdpositionNavigation);
            return employee.AsQueryable();
        }
        public async Task<IEnumerable<Employee>> GetQuery(int id)
        {

            var query = await buildingContext.Employees.Include(opt => opt.QueryProrabs).Where(c => c.EmployeeId == id).ToListAsync();

            return query.AsQueryable();
        }

        public async Task<EmployeesBuilding> GetEmployeeSite(int id)
        {
            return await buildingContext.EmployeesBuildings.FirstOrDefaultAsync(c => c.EmployeeId == id);

        }

        public async Task<Position> GetEmployeePosition(int positionId)
        {
            return await buildingContext.Positions.FirstOrDefaultAsync(c => c.Idposition == positionId);
        }
        public IQueryable<string> GetAllPosition()
        {
            return  buildingContext.Positions.Select(c => c.Name).AsQueryable();
        }

        public IQueryable<Employee> GetAllProprab()
        {
            return  buildingContext.Employees.Include(c => c.IdpositionNavigation).Where(c => c.IdpositionNavigation.Name == "Прораб");
        }
    }
}
