using Product.Domain.Entity;

namespace Product.Test.Test.Categories
{
    public class DeleteCategoryTest
    {
        private Fixture _fixture;
        private Mock<ICategoryRepository> _categoryRepositoryMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private CategoryService _categoryService;

        public DeleteCategoryTest()
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
        public async Task Delete_Category_Should_Succeed()
        {
            var categoryIdToDelete = _fixture.Create<long>();
            var category = _fixture.Create<Category>();
            category.Id = categoryIdToDelete;
            category.IsActive = true;

            _categoryRepositoryMock.Setup(repo => repo.GetById(It.IsAny<long>())).ReturnsAsync(category);
            _categoryRepositoryMock.Setup(repo => repo.Exist(It.IsAny<Expression<Func<Category, bool>>>()))
                .ReturnsAsync(true);

            await _categoryService.Delete(It.IsAny<long>());

            _categoryRepositoryMock.Verify(repo => repo.Update(It.Is<Category>(c => !c.IsActive)), Times.Once);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task Should_Throw_ValidationException_When_Invalid_Category()
        {

            _categoryRepositoryMock.Setup(repo => repo.Exist(It.IsAny<Expression<Func<Category, bool>>>()))
                .ReturnsAsync(false);

            await Should.ThrowAsync<ValidationException>(async () => await _categoryService.Delete(It.IsAny<long>()));

            _categoryRepositoryMock.Verify(repo => repo.Update(It.IsAny<Category>()), Times.Never);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
