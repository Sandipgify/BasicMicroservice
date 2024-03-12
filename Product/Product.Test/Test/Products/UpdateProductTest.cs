namespace Product.Test.Test.Products
{
    public class UpdateProductTest
    {
        private Mock<IProductRepository> _productRepositoryMock;
        private Mock<ICategoryRepository> _categoryRepositoryMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private ProductService _productService;
        private Fixture _fixture;
        private UpdateProductDTO _updateProductDTO;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _productRepositoryMock = new Mock<IProductRepository>();
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _productService = new ProductService(_unitOfWorkMock.Object, _productRepositoryMock.Object, _categoryRepositoryMock.Object);
            _updateProductDTO = _fixture.Create<UpdateProductDTO>();
        }

        [Test]
        public async Task Update_Product_Should_Succeed()
        {
            var productId = 1;
            _updateProductDTO.Id = productId;
            var product = _fixture.Create<Domain.Entity.Product>();
            _productRepositoryMock.Setup(repo => repo.GetById(It.IsAny<long>()))
                .ReturnsAsync(product);

            _productRepositoryMock.Setup(repo => repo.Exist(It.IsAny<Expression<Func<Domain.Entity.Product, bool>>>())).ReturnsAsync(true);

            _productRepositoryMock.Setup(repo => repo.ProductNameExist(It.IsAny<string>(), It.IsAny<long>())).ReturnsAsync(false);

            _categoryRepositoryMock.Setup(repo => repo.Exist(It.IsAny<Expression<Func<Domain.Entity.Category, bool>>>()))
                .ReturnsAsync(true);


            await _productService.Update(_updateProductDTO, productId);


            _productRepositoryMock.Verify(repo => repo.GetById(It.IsAny<long>()), Times.Once);
            _productRepositoryMock.Verify(repo => repo.Update(It.IsAny<Domain.Entity.Product>()), Times.Once);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);

            product.Name.ShouldBe(_updateProductDTO.Name);
            product.Description.ShouldBe(_updateProductDTO.Description);
            product.CategoryId.ShouldBe(_updateProductDTO.CategoryId);
            product.Price.ShouldBe(_updateProductDTO.Price);
        }

        [Test]
        public async Task Should_Throw_ValidationException_When_Invalid_Product()
        {
            var productId = 1;
            _updateProductDTO.Id = productId;
            var product = _fixture.Create<Domain.Entity.Product>();
            _productRepositoryMock.Setup(repo => repo.GetById(It.IsAny<long>()))
                .ReturnsAsync(product);

            _productRepositoryMock.Setup(repo => repo.Exist(It.IsAny<Expression<Func<Domain.Entity.Product, bool>>>())).ReturnsAsync(false);

            _productRepositoryMock.Setup(repo => repo.ProductNameExist(It.IsAny<string>(), It.IsAny<long>())).ReturnsAsync(false);

            _categoryRepositoryMock.Setup(repo => repo.Exist(It.IsAny<Expression<Func<Domain.Entity.Category, bool>>>()))
                .ReturnsAsync(true);

            await Should.ThrowAsync<ValidationException>(async () => await _productService.Update(_updateProductDTO, productId));

            
            _productRepositoryMock.Verify(repo => repo.Update(It.IsAny<Domain.Entity.Product>()), Times.Never);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task Should_Throw_ValidationException_When_Invalid_Category()
        {
            var productId = 1;
            _updateProductDTO.Id = productId;
            var product = _fixture.Create<Domain.Entity.Product>();
            _productRepositoryMock.Setup(repo => repo.GetById(It.IsAny<long>()))
                .ReturnsAsync(product);

            _productRepositoryMock.Setup(repo => repo.Exist(It.IsAny<Expression<Func<Domain.Entity.Product, bool>>>())).ReturnsAsync(true);

            _productRepositoryMock.Setup(repo => repo.ProductNameExist(It.IsAny<string>(), It.IsAny<long>())).ReturnsAsync(false);

            _categoryRepositoryMock.Setup(repo => repo.Exist(It.IsAny<Expression<Func<Domain.Entity.Category, bool>>>()))
                .ReturnsAsync(false);

            await Should.ThrowAsync<ValidationException>(async () => await _productService.Update(_updateProductDTO, productId));

            _productRepositoryMock.Verify(repo => repo.Update(It.IsAny<Domain.Entity.Product>()), Times.Never);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}