using Building.DAL.Interfaces;
using Building.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Building.DAL.Repositories
{
    public class EmployeeBuildingRepository : IEmployeeBuildingRepository
    {
        private readonly BuildingContext buildingContext;
        public EmployeeBuildingRepository(BuildingContext buildingContext)
        {
            this.buildingContext = buildingContext;
                
        }
        public bool Create(EmployeesBuilding entity)
        {
            if(entity != null)
            {
                buildingContext.EmployeesBuildings.Add(entity);
                buildingContext.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
