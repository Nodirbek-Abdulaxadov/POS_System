using BLL.Dtos.ProductDtos;
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
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductViewDto>>> Get()
        {
            try
            {
                var products = await _productService.GetAllAsync();
                return Ok(products);
            }
            catch (Exception ex) 
            { 
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("d")]
        public async Task<ActionResult<IEnumerable<ProductViewDto>>> DGet()
        {
            try
            {
                var products = await _productService.GetDProducts();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("paged")]
        public async Task<ActionResult<IEnumerable<ProductViewDto>>> Get(int pageSize, int pageNumber)
        {
            try
            {
                var list = await _productService.GetProductsAsync(pageSize, pageNumber);
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

        [HttpGet("archived/paged")]
        public async Task<ActionResult<IEnumerable<ProductViewDto>>> GetArchived(int pageSize, int pageNumber)
        {
            try
            {
                var list = await _productService.GetArchivedProductsAsync(pageSize, pageNumber);
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
        public async Task<ActionResult<ProductViewDto>> Get(int id)
        {
            try
            {
                var product = await _productService.GetByIdAsync(id);
                return Ok(product);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ProductViewDto>> Post(AddProductDto dto)
        {
            try
            {
                var model = await _productService.AddAsync(dto);
                return Ok(model);
            }
            catch(MarketException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<ProductViewDto>> Put(ProductUpdateDto dto)
        {
            try
            {
                var res = await _productService.UpdateAsync(dto);
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
                var model = await _productService.GetByIdAsync(id);
                await _productService.ActionAsync(id, ActionType.Remove);
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

        [HttpPut("archive/{id}")]
        public async Task<IActionResult> Archive(int id)
        {
            try
            {
                var model = await _productService.GetByIdAsync(id);
                await _productService.ActionAsync(id, ActionType.Archive);
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

        [HttpPut("recover/{id}")]
        public async Task<IActionResult> Recover(int id)
        {
            try
            {
                var model = await _productService.GetByIdAsync(id);
                await _productService.ActionAsync(id, ActionType.Recover);
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
