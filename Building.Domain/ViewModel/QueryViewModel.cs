using System.Web.Mvc;

namespace Building.Domain.ViewModel
{
    public class QueryViewModel
    {
        public string? CatalogName { get; set; }
        public int? CatalogId { get; set; }
        public string? MaterialName { get; set; }
        public int? MaterialId { get; set; }
        public string? Name { get; set; }
        public string? Measure { get; set; }
        public int? Count { get; set; }
        public string? Additional { get; set; }
        public string? Deadline { get; set; }
        public string? Site { get; set; }

    }
}
