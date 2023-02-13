using BLL.Dtos.ProductDtos;
using BLL.Dtos.WarehouseDtos;
using BLL.Interfaces;
using BLL.Services;
using BLL.Validations;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        [HttpGet("paged")]
        public async Task<ActionResult<IEnumerable<ProductViewDto>>> Get(int pageSize, int pageNumber)
        {
            try
            {
                var list = await _productService.GetProductsAsync(pageSize, pageNumber);
                return Ok(list);
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
    }
}
