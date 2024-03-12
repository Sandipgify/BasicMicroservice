namespace Product.Test.Test.Categories
{
    public class UpdateCategoryTest
    {
        private Fixture _fixture;
        private Mock<ICategoryRepository> _categoryRepositoryMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private CategoryService _categoryService;
        private UpdateCategoryDTO _updateCategoryDTO;

        public UpdateCategoryTest()
        {
            _fixture = new Fixture();
        }

        [SetUp]
        public void Setup()
        {
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _categoryService = new CategoryService(_unitOfWorkMock.Object, _categoryRepositoryMock.Object);
            _updateCategoryDTO = _fixture.Create<UpdateCategoryDTO>();

            _categoryRepositoryMock.Setup(repo => repo.Exist(It.IsAny<Expression<Func<Domain.Entity.Category, bool>>>()))
                .ReturnsAsync(true);
        }

        [Test]
        public async Task Update_Category_Should_Succeed()
        {
            var categoryId = 1;

            _categoryRepositoryMock.Setup(repo => repo.CategoryNameExist(It.IsAny<string>(),It.IsAny<long>())).ReturnsAsync(false);

            var category = _fixture.Create<Domain.Entity.Category>();

            _categoryRepositoryMock.Setup(repo => repo.GetById(It.IsAny<long>()))
                .ReturnsAsync(category);

            _updateCategoryDTO.Id = categoryId;

            await _categoryService.Update(_updateCategoryDTO, categoryId);

            _categoryRepositoryMock.Verify(repo => repo.GetById(It.IsAny<long>()), Times.Once);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);

            category.Name.ShouldBe(_updateCategoryDTO.Name);
        }

        [Test]
        public async Task Should_Throw_ValidationException_When_Invalid_Category()
        {
            var invalidCategoryId = 2;
            _categoryRepositoryMock.Setup(repo => repo.Exist(It.IsAny<Expression<Func<Domain.Entity.Category, bool>>>()))
                .ReturnsAsync(false);


            await Should.ThrowAsync<ValidationException>(async () => await _categoryService.Update(_updateCategoryDTO, invalidCategoryId));

            _categoryRepositoryMock.Verify(repo => repo.GetById(It.IsAny<long>()), Times.Never);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task Should_Throw_ValidationException_When_Duplicate_CategoryName()
        {
            _categoryRepositoryMock.Setup(repo => repo.Exist(It.IsAny<Expression<Func<Domain.Entity.Category, bool>>>()))
                .ReturnsAsync(true);

            await Should.ThrowAsync<ValidationException>(async () => await _categoryService.Update(_updateCategoryDTO, It.IsAny<long>()));

            _categoryRepositoryMock.Verify(repo => repo.GetById(It.IsAny<long>()), Times.Never);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
