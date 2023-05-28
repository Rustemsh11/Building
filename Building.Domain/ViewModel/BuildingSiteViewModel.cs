using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Building.Domain.ViewModel
{
    public class BuildingSiteViewModel
    {
        public int BuildingID { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public byte[]? Photo { get; set; }
        public string? Deadline { get; set; }
        public string? ProrabName { get; set; }
    }
}
