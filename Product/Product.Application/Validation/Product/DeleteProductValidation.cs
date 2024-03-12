namespace Product.Application.Validation.Product
{
    internal class DeleteProductValidation:AbstractValidator<long>
    {
        private readonly IProductRepository _productRepository;

        public DeleteProductValidation(IProductRepository productRepository)
        {
            _productRepository = productRepository;
            RuleFor(x => x).MustAsync(ProductExist)
          .WithMessage("Invalid product").WithName("id");
        }
        private async Task<bool> ProductExist(long id, CancellationToken cancellationToken)
        {
            return await _productRepository.Exist(x => x.Id == id && x.IsActive);
        }
    }
}


