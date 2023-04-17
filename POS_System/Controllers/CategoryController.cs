using BLL.Dtos.CategoryDtos;
using BLL.Dtos.WarehouseDtos;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
	{
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryViewDto>>> Get()
    {
        try
        {
            var list = await _categoryService.GetAllAsync();
            return Ok(list);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("paged")]
    public async Task<ActionResult<IEnumerable<CategoryViewDto>>> Get(int pageSize, int pageNumber)
    {
        try
        {
            var list = await _categoryService.GetCategoriesAsync(pageSize, pageNumber);
            var json = JsonConvert.SerializeObject(list, Formatting.Indented,
            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Ok(json);
        }
        catch (MarketException)
        {
            return Ok(new PagedList<CategoryViewDto>(new List<CategoryViewDto>(), 0, 0, 0));
        }
        catch (ArgumentNullException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("archived/paged")]
    public async Task<ActionResult<IEnumerable<CategoryViewDto>>> GetArchived(int pageSize, int pageNumber)
    {
        try
        {
            var list = await _categoryService.GetArchivedCategoriesAsync(pageSize, pageNumber);
            var json = JsonConvert.SerializeObject(list, Formatting.Indented,
            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Ok(json);
        }
        catch (MarketException)
        {
            return Ok(new PagedList<CategoryViewDto>(new List<CategoryViewDto>(), 0, 0, 0));
        }
        catch (ArgumentNullException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryViewDto>> Get(int id)
    {
        try
        {
            var model = await _categoryService.GetByIdAsync(id);
            return Ok(model);
        }
        catch (ArgumentNullException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<CategoryViewDto>> Post(AddCategoryDto model)
    {
        try
        {
            var result = await _categoryService.AddAsync(model);
            return StatusCode(201, result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    public async Task<ActionResult<CategoryViewDto>> Put(UpdateCategoryDto model)
    {
        try
        {
            var res = await _categoryService.UpdateAsync(model);
            return Ok(res);
        }
        catch (MarketException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(int id)
    {
        try
        {
            var model = await _categoryService.GetByIdAsync(id);
            await _categoryService.ActionAsync(id, ActionType.Remove);
            return Ok();
        }
        catch (ArgumentNullException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("archive/{id}")]
    public async Task<IActionResult> Archive(int id)
    {
        try
        {
            var model = await _categoryService.GetByIdAsync(id);
            await _categoryService.ActionAsync(id, ActionType.Archive);
            return Ok();
        }
        catch (ArgumentNullException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("recover/{id}")]
    public async Task<IActionResult> Recover(int id)
    {
        try
        {
            var model = await _categoryService.GetByIdAsync(id);
            await _categoryService.ActionAsync(id, ActionType.Recover);
            return Ok();
        }
        catch (ArgumentNullException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}