namespace Product.Application.Validation.Category
{
    internal class DeleteCategoryValidation:AbstractValidator<long>
    {
        private readonly ICategoryRepository _categoryRepository;

        public DeleteCategoryValidation(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
            RuleFor(x => x).MustAsync(async (name, cancellationToken) =>
           await CategoryExist(name, cancellationToken))
          .WithMessage("Invalid category").WithName("id");
        }
        private async Task<bool> CategoryExist(long id, CancellationToken cancellationToken)
        {
            return await _categoryRepository.Exist(x => x.Id == id && x.IsActive);
        }
    }
}


