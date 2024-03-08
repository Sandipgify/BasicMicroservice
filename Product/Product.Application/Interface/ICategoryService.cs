using Product.Application.DTO;

namespace Product.Application.Interface
{
    public interface ICategoryService
    {
        Task<long> Create(CategoryDTO requestDTO);
        Task Update(UpdateCategoryDTO requestDTO, long id);
        Task Delete(long categoryId);
        Task<IEnumerable<CategoryResponseDTO>> Get();
    }
}
