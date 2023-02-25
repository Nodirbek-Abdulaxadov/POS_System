using BLL.Dtos.WarehouseDtos;
using BLL.Dtos.WarehouseItemDtos;
using BLL.Interfaces;
using BLL.Services;
using BLL.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WarehouseItemController : ControllerBase
    {
        private readonly IWarehouseItemService _itemService;

        public WarehouseItemController(IWarehouseItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WarehouseItemDto>>> Get(int warehouseId)
        {
            try
            {
                var list = await _itemService.GetAllAsync(warehouseId);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("paged")]
        public async Task<ActionResult<IEnumerable<WarehouseItemDto>>> Get(int pageSize, int pageNumber, int warehouseId)
        {
            try
            {
                var list = await _itemService.GetPagedAsync(pageSize, pageNumber, warehouseId);
                return Ok(list);
            }
            catch (MarketException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WarehouseItemDto>> GetById(int id)
        {
            try
            {
                var model = await _itemService.GetByIdAsync(id);
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
        public async Task<ActionResult<WarehouseItemDto>> Post(AddWarehouseItemDto model)
        {
            try
            {
                var result = await _itemService.AddAsync(model);
                return StatusCode(201, result);
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
                var model = await _itemService.GetByIdAsync(id);
                await _itemService.RemoveAsync(id);
                return NoContent();
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
}