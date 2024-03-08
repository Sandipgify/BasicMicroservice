using Product.Application.DTO.Category;

namespace Product.Application.Interface
{
    public interface ICategoryService
    {
        Task<long> Create(CategoryDTO requestDTO);
        Task Update(CategoryDTO requestDTO, long id);
        Task Delete(long categoryId);
        Task<IEnumerable<CategoryResponseDTO>> Get();
    }
}
