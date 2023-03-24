using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Building.Domain.Entity
{
    public partial class Query
    {
        //public Query()
        //{
        //    Queries = new HashSet<QueryDetail>();
        //}
        public int QueryId { get; set; }
        public int? ProrabId { get; set; }
        public string? Name { get; set; }
        public string? Status { get; set; }
        public string? Deadline { get; set; }
        //public byte[]? Photo { get; set; }
        
        public int? SnabId { get; set; }   
        public int? SiteId { get; set; }
        
        public string? State { get; set; }
        public string? DeliveryDate { get; set; }
        public string? CreateDate { get; set; }


        public virtual Employee? Prorab { get; set; }
        public virtual BuildingSite? Site { get; set; }
        public virtual Employee? Snab { get; set; }
        //public virtual QueryDetail? QueryDetail { get; set; }


        public virtual ICollection<QueryDetail> QueryDetails { get; set; }
    }
}
