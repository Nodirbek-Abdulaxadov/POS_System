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

    public async Task<ReceiptDto> CreateAsync(string sellerId)
    {
        var receipt = new Receipt()
        {
            CreatedDate = "",
            SellerId = sellerId,
            Discount = 0,
            HasLoan = false,
            IsDeleted = false,
            PaidCard = 0,
            PaidCash = 0,
            TotalPrice = 0,
            Loan = new Loan(),
            Transactions = new List<Transaction>()
        };

        var model = await _unitOfWork.Receipts.AddAsync(receipt);
        await _unitOfWork.SaveAsync();
        return (ReceiptDto)receipt;
    }

    public async Task<ReceiptDto> SaveAsync(ReceiptDto receiptDto)
    {
        await _unitOfWork.Receipts.UpdateAsync((Receipt)receiptDto);
        await _unitOfWork.SaveAsync();

        return receiptDto;
    }
}