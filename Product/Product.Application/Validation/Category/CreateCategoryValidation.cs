using FluentValidation;
using Product.Application.DTO;
using Product.Application.Infrastructure;

namespace Product.Application.Validation.Category;

internal class CreateCategoryValidation : AbstractValidator<CategoryDTO>
{
    private readonly ICategoryRepository _categoryRepository;

    public CreateCategoryValidation(ICategoryRepository categoryRepository)
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required")
            .MustAsync(async (name, cancellationToken) =>
                !await CategoryExist(name, cancellationToken))
               .WithMessage("Category already exist");

        _categoryRepository = categoryRepository;
    }

    private async Task<bool> CategoryExist(string name, CancellationToken cancellationToken)
    {
        return await _categoryRepository.Exist(x => x.Name == name && x.IsActive);
    }
}
