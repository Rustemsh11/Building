using Building.DAL.Interfaces;
using Building.Domain.Entity;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Building.DAL.Repositories
{
    public class PositionRepository : IPositionRepository
    {
        private readonly BuildingContext buildingContext;
        public PositionRepository(BuildingContext buildingContext)
        {
            this.buildingContext = buildingContext;

        }
        public IQueryable<Position> GetIdPosition()
        {
            return buildingContext.Positions.AsQueryable();
        }
    }
}
