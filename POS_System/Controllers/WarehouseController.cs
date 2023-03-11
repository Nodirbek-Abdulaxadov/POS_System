using BLL.Dtos.WarehouseDtos;
using BLL.Dtos.WarehouseItemDtos;
using BLL.Interfaces;
using BLL.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class WarehouseController : ControllerBase
    {
        private readonly IWarehouseService _itemService;

        public WarehouseController(IWarehouseService warehouseService)
        {
            _itemService = warehouseService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WarehouseItemDto>>> Get()
        {
            try
            {
                var list = await _itemService.GetAllAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("paged")]
        public async Task<ActionResult<IEnumerable<WarehouseItemDto>>> Get(int pageSize, int pageNumber)
        {
            try
            {
                var list = await _itemService.GetWarehousesAsync(pageSize, pageNumber);
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
        public async Task<ActionResult<WarehouseItemDto>> Get(int id)
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
        public async Task<ActionResult<WarehouseItemDto>> Post(AddWarehouseDto model)
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

        [HttpPut]
        public async Task<ActionResult<WarehouseItemDto>> Put(WarehouseUpdateDto model)
        {
            try
            {
                var res = await _itemService.UpdateAsync(model);
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
