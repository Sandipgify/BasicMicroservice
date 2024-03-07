using Product.Application.DTO.Category;
using Product.Application.Infrastructure;
using Product.Application.Interface;
using Product.Application.Mapper;

namespace Product.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(IUnitOfWork unitOfWork,
            ICategoryRepository categoryRepository)
        {
            _unitOfWork = unitOfWork;
            _categoryRepository = categoryRepository;
        }

        public async Task<long> Create(CreateCategoryRequestDTO requestDTO)
        {
            var category = requestDTO.ToCategory();
            category.IsActive = true;
            category.CreatedAt = DateTime.UtcNow;
            await _categoryRepository.AddAsync(category);
            await _unitOfWork.SaveAsync();
            return category.Id;
        }
    }
}
