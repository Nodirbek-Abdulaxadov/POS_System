using BLL.Dtos.WarehouseItemDtos;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        public async Task<ActionResult<IEnumerable<WarehouseItemViewDto>>> Get()
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
        public async Task<IActionResult> Get(int pageSize, int pageNumber, int warehouseId)
        {
            try
            {
                var list = await _itemService.GetPagedAsync(pageSize, pageNumber, warehouseId);
                return Ok(list);
            }
            catch (MarketException)
            {
                 return Ok(new PagedList<WarehouseItemViewDto>(new List<WarehouseItemViewDto>(), 0, 0, 0));
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

        [HttpGet("paged/all")]
        public async Task<ActionResult<IEnumerable<WarehouseItemViewDto>>> GetAll()
        {
            try
            {
                var list = await _itemService.GetAllAsPaged();
                if (list.Data.Count == 0)
                {
                    list.Data = new List<WarehouseItemViewDto>();
                }

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

        [HttpGet("archived/paged")]
        public async Task<ActionResult<IEnumerable<WarehouseItemViewDto>>> GetArchived(int pageSize, int pageNumber)
        {
            try
            {
                var list = await _itemService.GetArchivedAsync(pageSize, pageNumber);
                var json = JsonConvert.SerializeObject(list, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return Ok(json);
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
        public async Task<ActionResult<WarehouseItemViewDto>> GetById(int id)
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
            catch (ArgumentNullException)
            {
                return StatusCode(410, "Null kelyaptida bro!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<WarehouseItemDto>> Put(UpdateWarehouseItemDto dto)
        {
            try
            {
                var result = await _itemService.Update(dto);
                return Ok(result);
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                var model = await _itemService.GetByIdAsync(id);
                await _itemService.ActionAsync(id, ActionType.Remove);
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
                var model = await _itemService.GetByIdAsync(id);
                await _itemService.ActionAsync(id, ActionType.Archive);
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
                var model = await _itemService.GetByIdAsync(id);
                await _itemService.ActionAsync(id, ActionType.Recover);
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
}