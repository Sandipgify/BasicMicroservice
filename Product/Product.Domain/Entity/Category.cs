namespace Product.Domain.Entity
{
    public class Category: BaseEntity
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
