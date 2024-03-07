using Product.Application.DTO.Category;
using Product.Domain.Entity;
using System.Runtime.CompilerServices;

namespace Product.Application.Mapper
{
    public static class CategoryMapper
    {
        public static Category ToCategory(this CreateCategoryRequestDTO requestDTO)
            => new Category
            {
                Name = requestDTO.Name
            };
    }
}
