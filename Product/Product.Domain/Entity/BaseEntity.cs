using System.ComponentModel.DataAnnotations;

namespace Product.Domain.Entity
{
    public class BaseEntity
    {
        [Key]
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
