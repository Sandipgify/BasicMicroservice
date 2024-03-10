using Product.Domain.Entity;

namespace Product.Application.Mapper
{
    public static class CategoryMapper
    {
        public static Category ToCategory(this CategoryDTO requestDTO)
            => new Category
            {
                Name = requestDTO.Name
            };

        public static CategoryResponseDTO ToCategoryResponse(this Category reqest)
           => new CategoryResponseDTO
           {
               Name = reqest.Name,
               CreatedDate = reqest.CreatedAt.ToString("yyyy-MM-dd"),
           };
    }
}
