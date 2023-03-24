using System;
using System.Collections.Generic;

namespace Building.Domain.Entity
{
    public partial class Employee
    {
        public Employee()
        {
            QueryProrabs = new HashSet<Query>();
            QuerySnabs = new HashSet<Query>();
        }

        public int EmployeeId { get; set; }
        public string Name { get; set; } = null!;
        public string SecondName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string? Birthday { get; set; }
        public string? Phone { get; set; }
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int Idposition { get; set; }
        public byte[]? Photo { get; set; }

        public virtual Position IdpositionNavigation { get; set; } = null!;
        public virtual ICollection<Query> QueryProrabs { get; set; }
        public virtual ICollection<Query> QuerySnabs { get; set; }
    }
}
