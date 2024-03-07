using Product.Application.DTO.Category;

namespace Product.Application.Interface
{
    public interface ICategoryService
    {
        Task<long> Create(CreateCategoryRequestDTO requestDTO);
    }
}
