using BLL.Dtos.ReceiptDtos;

namespace BLL.Interfaces;

public interface IReceiptService
{
    Task<ReceiptDto> AddAsync(AddReceiptDto receiptDto);
}