using FluentValidation;
using Product.Application.DTO;
using Product.Application.Infrastructure;

namespace Product.Application.Validation.Product
{
    public record UpdateProductValidationRequest(UpdateProductDTO productDTO, long id);
    internal class UpdateProductValidation : AbstractValidator<UpdateProductValidationRequest>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public UpdateProductValidation(IProductRepository productRepository,
            ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;

            RuleFor(x => x.id).Equal(x => x.productDTO.Id).WithMessage("Invalid product");

            RuleFor(x => x.id).MustAsync(ProductExist)
          .WithMessage("Invalid product");

            RuleFor(x => x.productDTO).MustAsync(async (product, cancellationToken) =>
          !await ProductNameExist(product, cancellationToken))
         .WithMessage("Product already exist");

            RuleFor(x => x.productDTO.CategoryId).GreaterThan(0).WithMessage("Invalid category")
          .MustAsync(CategoryExist)
          .WithMessage("Category not exist");

            RuleFor(x => x.productDTO.Price).GreaterThanOrEqualTo(0).WithMessage("Invalid price");
        }
        private async Task<bool> ProductExist(long id, CancellationToken cancellationToken)
        {
            return await _productRepository.Exist(x => x.Id == id && x.IsActive);
        }

        private async Task<bool> ProductNameExist(UpdateProductDTO productDTO, CancellationToken cancellationToken)
        {
            return await _productRepository.Exist(x => x.Id != productDTO.Id && x.Name == productDTO.Name && x.IsActive);
        }

        private async Task<bool> CategoryExist(long categoryId, CancellationToken cancellationToken)
        {
            return await _categoryRepository.Exist(x => x.Id == categoryId && x.IsActive);
        }
    }
}
