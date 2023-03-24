using System;
using System.Collections.Generic;

namespace Building.Domain.Entity
{
    public partial class BuildingSite
    {
        public BuildingSite()
        {
            Queries = new HashSet<Query>();
        }

        public int BuildingId { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? Photo { get; set; } 
        public string? Deadline { get; set; } 

        public virtual ICollection<Query> Queries { get; set; }
    }
}
