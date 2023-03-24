using System;
using System.Collections.Generic;

namespace Building.Domain.Entity
{
    public partial class EmployeesBuilding
    {
        public int? EmployeeId { get; set; }
        public int? BuildingId { get; set; }

        public virtual BuildingSite? Building { get; set; }
        public virtual Employee? Employee { get; set; }
    }
}
