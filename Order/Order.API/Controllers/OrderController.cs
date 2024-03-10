using Order.Application.DTO;
using Order.Application.Interface;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;

        public OrderController(ILogger<OrderController> logger,
            IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }
        [HttpPost]
        [SwaggerOperation(
        Summary = "Create order",
        Description = "Create a new order",
        OperationId = "order.create",
        Tags = new[] { "Order" })]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(OrderDTO request)
        {
            try
            {
                long orderId = await _orderService.Create(request);
                return Ok();
                //return CreatedAtAction(nameof(Get), new { id = orderId }, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Update order",
            Description = "Update an existing order",
            OperationId = "order.update",
            Tags = new[] { "Order" })]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(UpdateOrderDTO request, long id)
        {
            try
            {
                await _orderService.Update(request, id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Get orders",
            Description = "Get a list of orders",
            OperationId = "order.get",
            Tags = new[] { "Order" })]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IEnumerable<OrderResponseDTO>))]
        public async Task<IActionResult> Get()
        {
            try
            {
                var orders = await _orderService.Get();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Delete order",
            Description = "Delete an existing order",
            OperationId = "order.delete",
            Tags = new[] { "order" })]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _orderService.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }


    }
}
