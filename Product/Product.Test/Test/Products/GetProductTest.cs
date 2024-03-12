using Product.Domain.Entity;

namespace Product.Test.Test.Products
{
    public class GetProductTest
    {
        private Mock<ICategoryRepository> _categoryRepositoryMock;
        private CategoryService _categoryService;
        private Fixture _fixture;
        private List<Category> _categoryFixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _categoryService = new CategoryService(null, _categoryRepositoryMock.Object);
            _categoryFixture = _fixture.Build<Category>()
                                .With(c => c.IsActive, true)
                                .CreateMany().ToList();
            _categoryRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                .ReturnsAsync(_categoryFixture);
        }

        [Test]
        public async Task Get_Categories_Should_Return_CategoryResponses()
        {
            var result = await _categoryService.Get();
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.Count().ShouldBe(_categoryFixture.Count);
        }

        [Test]
        public async Task Get_Categories_Should_Return_Empty_List_When_No_Categories()
        {
            _categoryRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                .ReturnsAsync(new List<Category>());

            var result = await _categoryService.Get();
            result.ShouldNotBeNull();
            result.ShouldBeEmpty();
        }
    }
}