namespace Product.Test.Test.Products
{
    public class DeleteProductTest
    {
        private Fixture _fixture;
        private Mock<IProductRepository> _productRepositoryMock;
        private Mock<ICategoryRepository> _categoryRepositorMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private ProductService _productService;

        public DeleteProductTest()
        {
            _fixture = new Fixture();
        }

        [SetUp]
        public void Setup()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _categoryRepositorMock = new Mock<ICategoryRepository>();
            _productService = new ProductService(_unitOfWorkMock.Object, _productRepositoryMock.Object, _categoryRepositorMock.Object);
        }

        [Test]
        public async Task Delete_Product_Should_Succeed()
        {
            var productId = 1;
            var product = _fixture.Create<Domain.Entity.Product>();

            _productRepositoryMock.Setup(repo => repo.GetById(It.IsAny<long>()))
                .ReturnsAsync(product);

            _productRepositoryMock.Setup(repo => repo.Exist(It.IsAny<Expression<Func<Domain.Entity.Product, bool>>>()))
                .ReturnsAsync(true);

            await _productService.Delete(productId);

            _productRepositoryMock.Verify(repo => repo.GetById(It.IsAny<long>()), Times.Once);
            _productRepositoryMock.Verify(repo => repo.Update(It.IsAny<Domain.Entity.Product>()), Times.Once);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);

            product.IsActive.ShouldBeFalse();
        }

        [Test]
        public async Task Should_Throw_ValidationException_When_Invalid_ProductId()
        {
            var invalidProductId = 2;
            _productRepositoryMock.Setup(repo => repo.Exist(It.IsAny<Expression<Func<Domain.Entity.Product, bool>>>()))
                .ReturnsAsync(false);

            await Should.ThrowAsync<ValidationException>(async () => await _productService.Delete(invalidProductId));

            _productRepositoryMock.Verify(repo => repo.GetById(It.IsAny<long>()), Times.Never);
            _productRepositoryMock.Verify(repo => repo.Update(It.IsAny<Domain.Entity.Product>()), Times.Never);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
