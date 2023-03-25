using DataLayer.Entities.Selling;

namespace BLL.Dtos.ReceiptDtos;

public class AddReceiptDto
{
    public decimal TotalPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal PaidCash { get; set; }
    public decimal PaidCard { get; set; }
    public bool HasLoan { get; set; }
    public string SellerId { get; set; } = string.Empty;
    public Loan Loan = new Loan();
    public List<string> Barcodes = new List<string>();
}