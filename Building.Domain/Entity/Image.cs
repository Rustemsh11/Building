namespace Building.Domain.Entity
{
    public class Image
    {
        public int Id { get; set; }
        public int? QueryId { get; set; }
        public byte[]? Photo { get; set; }

        public virtual Query? Query { get; set; }
    }
}
