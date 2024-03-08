using FluentValidation;
using Product.Application.DTO;
using Product.Application.Infrastructure;

namespace Product.Application.Validation.Product;

internal class CreateProductValidation : AbstractValidator<ProductDTO>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public CreateProductValidation(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required")
            .MustAsync(async (name, cancellationToken) =>
            !await ProductExist(name, cancellationToken))
            .WithMessage("Product already exist");

        RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage("Invalid category")
           .MustAsync(CategoryExist)
           .WithMessage("Category not exist");

        RuleFor(x => x.Price).GreaterThanOrEqualTo(0).WithMessage("Invalid price");
    }

    private async Task<bool> ProductExist(string name, CancellationToken cancellationToken)
    {
        return await _productRepository.Exist(x => x.Name == name && x.IsActive);
    }

    private async Task<bool> CategoryExist(long categoryId, CancellationToken cancellationToken)
    {
        return await _categoryRepository.Exist(x => x.Id == categoryId && x.IsActive);
    }
}
