using BLL.Dtos.ReceiptDtos;
using BLL.Interfaces;
using DataLayer.Entities.Selling;
using DataLayer.Interfaces;

namespace BLL.Services;

public class ReceiptService : IReceiptService
{
    private readonly IUnitOfWork _unitOfWork;

    public ReceiptService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public async Task<ReceiptDto> AddAsync(AddReceiptDto receiptDto)
    {
        var receipt = await _unitOfWork.Receipts.AddAsync((Receipt)receiptDto);
        await _unitOfWork.SaveAsync();

        foreach (var item in receiptDto.Transactions)
        {
            var transaction = await _unitOfWork.Transactions.AddAsync((Transaction)item);
            await _unitOfWork.SaveAsync();
            transaction.ReceiptId = receipt.Id;
            transaction.ProductId = (await _unitOfWork.Products.GetAllAsync())
                                           .FirstOrDefault(p => p.Barcode == item.Barcode)
                                           .Id;
            await _unitOfWork.Transactions.UpdateAsync(transaction);
            await _unitOfWork.SaveAsync();

            var warehouseItem = (await _unitOfWork.WarehouseItems.GetAllAsync())
                                .FirstOrDefault(i => i.ProductId == transaction.ProductId);
            warehouseItem.Quantity -= item.Quantity;
            await _unitOfWork.WarehouseItems.UpdateAsync(warehouseItem);
            await _unitOfWork.SaveAsync();
        }

        return (ReceiptDto)receipt;
    }
}