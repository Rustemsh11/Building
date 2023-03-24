using System;
using System.Collections.Generic;

namespace Building.Domain.Entity
{
    public partial class Material
    {
        //public Material()
        //{
        //    Queries = new HashSet<Query>();
        //}

        public int MaterialId { get; set; }
        public string Name { get; set; } = null!;
        public int? IdCatalog { get; set; }
        

        //public string Photo { get; set; }        
        //public virtual ICollection<Query> Queries { get; set; }
        public virtual ICollection<QueryDetail> QueryDetails { get; set; }
        public virtual Catalog Catalog { get; set; } = null!;
    }
}
