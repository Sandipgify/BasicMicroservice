using Product.Application.Validation.Product;

namespace Product.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductService(IUnitOfWork unitOfWork,
            IProductRepository productRepository,
            ICategoryRepository categoryRepository)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<long> Create(ProductDTO requestDTO)
        {
            #region Validation
            var validation = new CreateProductValidation(_productRepository, _categoryRepository);
            await validation.ValidateAndThrowAsync(requestDTO);
            #endregion

            var product = requestDTO.ToProduct();
            product.IsActive = true;
            product.CreatedAt = DateTime.UtcNow;
            await _productRepository.AddAsync(product);
            await _unitOfWork.SaveAsync();

            return product.Id;
        }

        public async Task Update(UpdateProductDTO requestDTO, long id)
        {
            #region Validation
            var validation = new UpdateProductValidation(_productRepository, _categoryRepository);
            await validation.ValidateAndThrowAsync(new UpdateProductValidationRequest(requestDTO, id));
            #endregion

            var product = await _productRepository.GetById(id);
            product.Name = requestDTO.Name;
            product.Description = requestDTO.Description;
            product.CategoryId = requestDTO.CategoryId;
            product.Price = requestDTO.Price;
            _productRepository.Update(product);

            await _unitOfWork.SaveAsync();
        }

        public async Task Delete(long productId)
        {
            #region Validation
            var validation = new DeleteProductValidation(_productRepository);
            await validation.ValidateAndThrowAsync(productId);
            #endregion

            var product = await _productRepository.GetById(productId);
            product.IsActive = false;
            _productRepository.Update(product);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<ProductResponseDTO>> Get()
        {
            var products = await _productRepository.GetAllAsync(x => x.IsActive);
            var productResponses = products.Select(x => x.ToProductResponse());
            return productResponses;
        }

        public async Task UpdateAvailableQuantity(long prouductId, UpdateAvailableQuantityDTO update)
        {
            var product = await _productRepository.GetById(prouductId);
            if (update.Type == 1)
            {
                product.QuantityAvailable += update.Quantity;
            }
            else
            {
                product.QuantityAvailable -= update.Quantity;
            }
            _productRepository.Update(product);
            await _unitOfWork.SaveAsync();
        }

    }
}
