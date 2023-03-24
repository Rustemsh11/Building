namespace Building.Domain.Entity
{
    public class Catalog
    {
        public int CatalogID { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Material> Materials { get; set; }
        //public virtual ICollection<QueryDetail> QueryDetails { get; set; }
        

    }
}
