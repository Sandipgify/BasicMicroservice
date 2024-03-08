using Product.Application.DTO;
using System;

namespace Product.Application.Mapper
{
    public static class ProductMapper
    {
        public static Domain.Entity.Product ToProduct(this ProductDTO requestDTO)
        => new Domain.Entity.Product
        {
            Name = requestDTO.Name,
            Description = requestDTO.Description,
            Price = requestDTO.Price,
            CategoryId = requestDTO.CategoryId
        };

        public static ProductResponseDTO ToProductResponse(this Domain.Entity.Product product)
            => new ProductResponseDTO
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                QuantityAvailable = product.QuantityAvailable,
                CategoryId = product.CategoryId,
                CreatedDate = product.CreatedAt.ToString("yyyy-MM-dd"),
            };
    }
}
