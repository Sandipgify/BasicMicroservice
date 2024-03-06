﻿namespace Product.Domain.Entity
{
    public class Product : BaseEntity
    {
        public long ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int QuantityAvailable { get; set; }
        public long CategoryId { get; set; }
        public bool IsActive { get; set; }
        public Category Category { get; set; }
    }
}