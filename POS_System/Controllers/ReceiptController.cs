using BLL.Dtos.ReceiptDtos;
using BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptController : ControllerBase
    {
        private readonly IReceiptService _receiptService;

        public ReceiptController(IReceiptService receiptService)
        {
            _receiptService = receiptService;
        }

        [HttpGet("{sellerId}")]
        public async Task<ActionResult<ReceiptDto>> Get(string sellerId)
        {
            try
            {
                var model = await _receiptService.CreateAsync(sellerId);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<ReceiptDto>> Put(ReceiptDto dto)
        {
            try
            {
                var model = await _receiptService.SaveAsync(dto);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}