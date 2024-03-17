namespace Order.Test.Test.Order
{
    public class DeleteOrderTest
    {
        private Fixture _fixture;
        private Mock<IOrderRepository> _orderRepositoryMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private OrderService _orderService;
        public DeleteOrderTest()
        {
            _fixture = new Fixture();
        }

        [SetUp]
        public void Setup()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _orderService = new OrderService(_unitOfWorkMock.Object, _orderRepositoryMock.Object, null);
            var order = _fixture.Create<Domain.Entity.Order>();
            _orderRepositoryMock.Setup(repo => repo.GetById(It.IsAny<long>())).ReturnsAsync(order);

        }


        [Test]
        public async Task Delete_Order_Should_Succeed()
        {
            _orderRepositoryMock
            .Setup(repo => repo.Exist(It.IsAny<Expression<Func<Domain.Entity.Order, bool>>>()))
            .ReturnsAsync(true);

            await _orderService.Delete(_fixture.Create<long>());

            _orderRepositoryMock.Verify(repo => repo.Update(It.Is<Domain.Entity.Order>(o => o.IsActive == false)), Times.Once);

            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task Should_Throw_Validation_When_Invalid_Order()
        {
            _orderRepositoryMock.Setup(repo => repo.Exist(It.IsAny<Expression<Func<Domain.Entity.Order, bool>>>())).ReturnsAsync(false);
            Assert.ThrowsAsync<ValidationException>(() => _orderService.Delete(It.IsAny<long>()));

        }
    }
}
