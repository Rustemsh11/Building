using Building.Domain.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Building.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class QueryFilterModel
    {
        public IEnumerable<Query> Queries { get; set; }
        public SelectList State { get; set; }
       
    }
}
