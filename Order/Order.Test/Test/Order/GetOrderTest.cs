using Order.Domain.Entity;
using Shouldly;

namespace Order.Test.Test.Order
{
    public class GetOrderTest
    {
        private Mock<IOrderRepository> _orderRepositoryMock;
        private OrderService _orderService;
        private Fixture _fixture;
        private List<Domain.Entity.Order> _orderFixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _orderService = new OrderService(null, _orderRepositoryMock.Object, null);
            _orderFixture = _fixture.Create<List<Domain.Entity.Order>>();
            _orderRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(_orderFixture);
        }

        [Test]
        public async Task Get_Orders_Should_Return_OrderResponses()
        {



            var result = await _orderService.Get();
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.Count().ShouldBe(_orderFixture.Count);
        }

        [Test]
        public async Task Get_Orders_Should_Return_Empty_List_When_No_Orders()
        {
            _orderRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Domain.Entity.Order>());
            var result = await _orderService.Get();
            result.ShouldNotBeNull();
            result.ShouldBeEmpty();
        }
    }
}
