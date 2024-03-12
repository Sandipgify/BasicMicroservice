using Order.Application.DTO;
using Order.Application.Validation;
using Shouldly;

namespace Order.Test.Test.Order
{
    public class UpdateOrderTest
    {
        private Fixture _fixture;
        private Mock<IOrderRepository> _orderRepositoryMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private OrderService _orderService;
        private UpdateOrderDTO _updateOrder;
        public UpdateOrderTest()
        {
            _fixture = new Fixture();
        }

        [SetUp]
        public void Setup()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _orderService = new OrderService(_unitOfWorkMock.Object, _orderRepositoryMock.Object);
            _updateOrder = _fixture.Create<UpdateOrderDTO>();

            _orderRepositoryMock.Setup(repo => repo.Exist(It.IsAny<Expression<Func<Domain.Entity.Order, bool>>>()))
                .ReturnsAsync(true);
        }

        [Test]
        public async Task Update_Order_Should_Succeed()
        {
            var order = _fixture.Create<Domain.Entity.Order>();

            _orderRepositoryMock
            .Setup(repo => repo.Exist(It.IsAny<Expression<Func<Domain.Entity.Order, bool>>>()))
            .ReturnsAsync(true);

            _orderRepositoryMock.Setup(repo => repo.GetById(It.IsAny<long>()))
               .ReturnsAsync(order);

            await _orderService.Update(_updateOrder, _updateOrder.OrderId);

            _orderRepositoryMock.Verify(repo => repo.GetById(It.IsAny<long>()), Times.Once);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task Should_Throw_ValidationException_When_Invalid_Order()
        {

            _orderRepositoryMock.Setup(repo => repo.Exist(It.IsAny<Expression<Func<Domain.Entity.Order, bool>>>()))
                .ReturnsAsync(false);

            await Should.ThrowAsync<ValidationException>(async () => await _orderService.Update(_updateOrder, It.IsAny<long>()));

            _orderRepositoryMock.Verify(repo => repo.GetById(It.IsAny<long>()), Times.Never);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task Should_Throw_ValidationException_When_Invalid_Product()
        {

            _updateOrder.OrderItems.First().ProductId = 0;


            await Should.ThrowAsync<ValidationException>(async () => await _orderService.Update(_updateOrder, It.IsAny<long>()));

            _orderRepositoryMock.Verify(repo => repo.GetById(It.IsAny<long>()), Times.Never);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task Should_Throw_ValidationException_When_Negative_Quantity()
        {

            _updateOrder.OrderItems.First().Quantity = -1;

            await Should.ThrowAsync<ValidationException>(async () => await _orderService.Update(_updateOrder, It.IsAny<long>()));

            _orderRepositoryMock.Verify(repo => repo.GetById(It.IsAny<long>()), Times.Never);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task Should_Throw_ValidationException_When_Negative_Price()
        {
            _updateOrder.OrderItems.First().Price = -10.0m;


            await Should.ThrowAsync<ValidationException>(async () => await _orderService.Update(_updateOrder, It.IsAny<long>()));

            _orderRepositoryMock.Verify(repo => repo.GetById(It.IsAny<long>()), Times.Never);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
