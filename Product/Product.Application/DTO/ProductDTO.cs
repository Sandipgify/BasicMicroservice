using Product.Domain.Entity;
using System;

namespace Product.Application.DTO
{
    public class ProductDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public long CategoryId { get; set; }
    }

    public class UpdateProductDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public long CategoryId { get; set; }
    }

    public class ProductResponseDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal QuantityAvailable { get; set; }
        public long CategoryId { get; set; }
        public string CreatedDate { get; set; }
    }

    public class UpdateAvailableQuantityDTO
    {
        public decimal Quantity { get; set; }
        public int Type { get; set; }
    }
}