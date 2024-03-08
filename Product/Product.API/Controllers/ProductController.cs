namespace Product.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly IProductService _productService;

        public ProductController(IProductService productService, ILogger<CategoryController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpPost]
        [SwaggerOperation(
        Summary = "Create product",
        Description = "Create a new product",
        OperationId = "product.create",
        Tags = new[] { "Product" })]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(ProductDTO request)
        {
            try
            {
                long productId = await _productService.Create(request);
                return Ok();
                //return CreatedAtAction(nameof(Get), new { id = productId }, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Update product",
            Description = "Update an existing product",
            OperationId = "product.update",
            Tags = new[] { "Product" })]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(UpdateProductDTO request, long id)
        {
            try
            {
                await _productService.Update(request, id);
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
            Summary = "Get products",
            Description = "Get a list of products",
            OperationId = "product.get",
            Tags = new[] { "Product" })]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IEnumerable<ProductResponseDTO>))]
        public async Task<IActionResult> Get()
        {
            try
            {
                var products = await _productService.Get();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Delete product",
            Description = "Delete an existing product",
            OperationId = "product.delete",
            Tags = new[] { "Product" })]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _productService.Delete(id);
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
