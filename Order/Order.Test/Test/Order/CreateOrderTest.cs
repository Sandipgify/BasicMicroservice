using Order.Application.DTO;
using Order.Application.Provider.Interfaces;
using Shouldly;

namespace Order.Test.Test.Order
{
    public class CreateOrderTest
    {
        private Fixture _fixture;
        private Mock<IOrderRepository> _orderRepositoryMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private OrderService _orderService;
        private Mock<IProducerProvider> _kafkaProducerServiceMock;

        public CreateOrderTest()
        {
            _fixture = new Fixture();
        }

        [SetUp]
        public void Setup()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _orderService = new OrderService(_unitOfWorkMock.Object, _orderRepositoryMock.Object, _kafkaProducerServiceMock.Object);
        }

        [Test]
        public async Task Create_Order_Should_Succeed()
        {
            var order = _fixture.Create<OrderDTO>();

            await _orderService.Create(order);

            _orderRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Domain.Entity.Order>()), Times.Once);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }


        [Test]
        public async Task Should_Throw_ValidationException_When_Negative_Quantity()
        {
            var invalidOrderDTO = _fixture.Create<OrderDTO>();
            invalidOrderDTO.OrderItems.First().Quantity = -1;

            await Should.ThrowAsync<ValidationException>(async () => await _orderService.Create(invalidOrderDTO));

            _orderRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Domain.Entity.Order>()), Times.Never);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task Should_Throw_ValidationException_When_Negative_Price()
        {
            var invalidOrderDTO = _fixture.Create<OrderDTO>();
            invalidOrderDTO.OrderItems.First().Price = -10.0m;

            await Should.ThrowAsync<ValidationException>(async () => await _orderService.Create(invalidOrderDTO));

            _orderRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Domain.Entity.Order>()), Times.Never);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task Should_Throw_ValidationException_When_Invalid_Product()
        {
            var invalidOrderDTO = _fixture.Create<OrderDTO>();
            invalidOrderDTO.OrderItems.First().ProductId = 0;

            await Should.ThrowAsync<ValidationException>(async () => await _orderService.Create(invalidOrderDTO));

            _orderRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Domain.Entity.Order>()), Times.Never);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
