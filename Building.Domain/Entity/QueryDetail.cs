using System.ComponentModel.DataAnnotations.Schema;

namespace Building.Domain.Entity
{
    public class QueryDetail
    {
        
        public int ID { get; set; }
        public int? QueryId { get; set; }
        public int? CatalogId { get; set; }
        public int? MaterialId { get; set; }
        public string? Name { get; set; }
        public int? Count { get; set; }
        public string? Measure { get; set; }
        public string? Additional { get; set; }
        public string? State { get; set; }
        public string? CreateDate { get; set; }
        public string? DeliveryDate { get; set; }
        public string? Comment { get; set; }


        public virtual Query? Query { get; set; }
        public virtual Material? Material { get; set; }
        //public virtual Catalog? Catalog { get; set; }

    }
}
