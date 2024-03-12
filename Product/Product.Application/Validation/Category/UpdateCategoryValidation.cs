namespace Product.Application.Validation.Category
{
    public record UpdateCategoryValidationRequest(UpdateCategoryDTO CategoryDTO, long id);
    internal class UpdateCategoryValidation : AbstractValidator<UpdateCategoryValidationRequest>
    {
        private readonly ICategoryRepository _categoryRepository;

        public UpdateCategoryValidation(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;

            RuleFor(x => x.id).Equal(x => x.CategoryDTO.Id).WithMessage("Invalid category");

            RuleFor(x => x.id).MustAsync(async (id, cancellationToken) =>
           await CategoryExist(id, cancellationToken))
          .WithMessage("Invalid category");

            RuleFor(x => x.CategoryDTO).MustAsync(async (category, cancellationToken) =>
          !await CategoryNameExist(category, cancellationToken))
         .WithMessage("Category already exist");
        }
        private async Task<bool> CategoryExist(long id, CancellationToken cancellationToken)
        {
            return await _categoryRepository.Exist(x => x.Id == id && x.IsActive);
        }

        private async Task<bool> CategoryNameExist(UpdateCategoryDTO categoryDTO, CancellationToken cancellationToken)
        {
            return await _categoryRepository.CategoryNameExist(categoryDTO.Name,categoryDTO.Id);
        }
    }
}
