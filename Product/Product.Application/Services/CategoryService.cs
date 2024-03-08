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

        public async Task<long> Create(CategoryDTO requestDTO)
        {
            var category = requestDTO.ToCategory();
            category.IsActive = true;
            category.CreatedAt = DateTime.UtcNow;
            await _categoryRepository.AddAsync(category);
            await _unitOfWork.SaveAsync();
            return category.Id;
        }

        public async Task Update(CategoryDTO requestDTO, long id)
        {
            var category = await _categoryRepository.GetById(id);
            category.Name = requestDTO.Name;
            _categoryRepository.Update(category);
            await _unitOfWork.SaveAsync();
        }

        public async Task Delete(long categoryId)
        {
            var category = await _categoryRepository.GetById(categoryId);
            category.IsActive = false;
            _categoryRepository.Update(category);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<CategoryResponseDTO>> Get()
        {
            var category = await _categoryRepository.GetAllAsync(x=>x.IsActive);
            var categoryResponse = category.Select(x => x.ToCategoryResponse());
            return categoryResponse;
        }
    }
}
