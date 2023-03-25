using BLL.Dtos.TransactionDtos;
using DataLayer.Entities.Selling;

namespace BLL.Dtos.ReceiptDtos;

public class ReceiptDto : BaseDto
{
    public string CreatedDate { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal PaidCash { get; set; }
    public decimal PaidCard { get; set; }
    public bool HasLoan { get; set; }
    public string SellerId { get; set; } = string.Empty;
    public Loan Loan = new Loan();
    public List<TransactionDto> Transactions = new List<TransactionDto>();

    public static explicit operator ReceiptDto(Receipt v)
        => new ReceiptDto()
        {
            CreatedDate = v.CreatedDate,
            TotalPrice = v.TotalPrice,
            Discount = v.Discount,
            PaidCash = v.PaidCash,
            PaidCard = v.PaidCard,
            HasLoan = v.HasLoan,
            Id = v.Id,
            Loan = v.Loan,
            SellerId = v.SellerId
        };

    public static explicit operator Receipt(ReceiptDto v)
        => new Receipt()
        {
            CreatedDate = v.CreatedDate,
            TotalPrice = v.TotalPrice,
            Discount = v.Discount,
            PaidCash = v.PaidCash,
            PaidCard = v.PaidCard,
            HasLoan = v.HasLoan,
            Id = v.Id,
            Loan = v.Loan,
            SellerId = v.SellerId
        };
}
