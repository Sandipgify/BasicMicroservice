namespace Product.Domain.Entity
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal QuantityAvailable { get; set; }
        public long CategoryId { get; set; }
        public bool IsActive { get; set; }
        public Category Category { get; set; }
    }
}
