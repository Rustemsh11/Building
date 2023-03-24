using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Building.Domain.DTO
{
    public class QueryDTO
    {
        public int QueryId { get; set; }
        public string? Name { get; set; }
        public string? Deadline { get; set; }
        public string? State { get; set; }
        public string? Responsible { get; set; }
        public SelectListItem CheckBox { get; set; }
    }
}
