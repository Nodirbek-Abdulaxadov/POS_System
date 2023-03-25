using BLL.Dtos.ReceiptDtos;
using BLL.Dtos.TransactionDtos;
using BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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

        [HttpPost]
        public async Task<ActionResult<ReceiptDto>> Put([FromQuery] AddReceiptDto dto, 
            [FromBody] List<TransactionDto> transactions)
        {
            try
            {
                dto.Transactions = transactions;
                var model = await _receiptService.AddAsync(dto);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}