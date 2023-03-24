using Building.Domain.Entity;
namespace Building.Domain.ViewModel
{
    public class CreateDataViewModel
    {
        public IEnumerable<Catalog> catalogs { get; set; }
        public IEnumerable<Material> materials { get; set; }
    }
}
