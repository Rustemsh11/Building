using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Building.Domain.DTO
{
    public class EmployeeDTO
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; } = null!;
        public string SecondName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string? Birthday { get; set; }
        public string? Phone { get; set; }
        public byte[]? Photo { get; set; }
        public int Idposition { get; set; }
    }
}
