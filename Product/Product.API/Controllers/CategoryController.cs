using Microsoft.AspNetCore.Authorization;
using Serilog;

namespace Product.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
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
        OperationId = "category.create",
       Tags = new[] { "Category" })]
    [SwaggerResponse(StatusCodes.Status201Created)]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CategoryDTO request)
    {
        try
        {
            long categoryId = await _categoryService.Create(request);
            return Ok();
        }
        catch (Exception ex)
        {
            Log.Error("{@errorMsg} \n{@exception}\n", ex.Message, ex);
            throw;
        }

    }

    [HttpPut]
    [SwaggerOperation(
        Summary = "Update category",
        Description = "update product category",
        OperationId = "category.update",
       Tags = new[] { "Category" })]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(UpdateCategoryDTO request, long id)
    {
        try
        {
            await _categoryService.Update(request, id);
            return NoContent();
        }
        catch (Exception ex)
        {
            Log.Error("{@errorMsg} \n{@exception}\n", ex.Message, ex);
            throw;
        }

    }

    [HttpGet]
    [SwaggerOperation(
       Summary = "Get category",
       Description = "Get category list",
       OperationId = "category.get",
       Tags = new[] { "Category" })]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IEnumerable<CategoryResponseDTO>))]
    public async Task<IActionResult> Get()
    {
        try
        {
            var category = await _categoryService.Get();
            return Ok(category);
        }
        catch (Exception ex)
        {
            Log.Error("{@errorMsg} \n{@exception}\n", ex.Message, ex);
            throw;
        }

    }

    [HttpDelete]
    [SwaggerOperation(
       Summary = "Delete category",
       Description = "Delete product category",
       OperationId = "category.delete",
       Tags = new[] { "Category" })]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(long id)
    {
        try
        {
            await _categoryService.Delete(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            Log.Error("{@errorMsg} \n{@exception}\n", ex.Message, ex);
            throw;
        }

    }
}
