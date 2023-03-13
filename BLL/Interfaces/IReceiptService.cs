using BLL.Dtos.ReceiptDtos;

namespace BLL.Interfaces;

public interface IReceiptService
{
    Task<ReceiptDto> CreateAsync(string sellerId);
    Task<ReceiptDto> SaveAsync(ReceiptDto receiptDto);
}