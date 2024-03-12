namespace Product.Test.Test.Categories
{
    public class CreateProductTest
    {
        private Fixture _fixture;
        private Mock<ICategoryRepository> _categoryRepositoryMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private CategoryService _categoryService;

        public CreateProductTest()
        {
            _fixture = new Fixture();
        }

        [SetUp]
        public void Setup()
        {
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _categoryService = new CategoryService(_unitOfWorkMock.Object, _categoryRepositoryMock.Object);
        }

        [Test]
        public async Task Create_Category_Should_Succeed()
        {
            var categoryDTO = _fixture.Create<CategoryDTO>();

            _categoryRepositoryMock.Setup(repo => repo.Exist(It.IsAny<Expression<Func<Domain.Entity.Category, bool>>>())).ReturnsAsync(false);

            await _categoryService.Create(categoryDTO);

           
            _categoryRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Domain.Entity.Category>()), Times.Once);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task Should_Throw_ValidationException_When_Duplicate_Category()
        {
            var duplicateCategoryDTO = _fixture.Create<CategoryDTO>();
            _categoryRepositoryMock.Setup(repo => repo.Exist(It.IsAny<Expression<Func<Domain.Entity.Category, bool>>>())).ReturnsAsync(true);

            await Should.ThrowAsync<ValidationException>(async () => await _categoryService.Create(duplicateCategoryDTO));

            _categoryRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Domain.Entity.Category>()), Times.Never);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
