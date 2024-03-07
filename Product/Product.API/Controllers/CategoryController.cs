using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Product.Application.DTO.Category;
using Product.Application.Interface;
using Swashbuckle.AspNetCore.Annotations;

namespace Product.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryService _categoryService;

        public CategoryController(ILogger<CategoryController> logger,
            ICategoryService categoryService)
        {
            _logger = logger;
            _categoryService = categoryService;
        }
        [HttpPost]
        [SwaggerOperation(
            Summary = "Create category",
            Description = "Create product category",
            OperationId = "Customer.create")]
        [SwaggerResponse(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(CreateCategoryRequestDTO request)
        {
            try
            {
                long categoryId = await _categoryService.Create(request);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            
        }
    }
}
