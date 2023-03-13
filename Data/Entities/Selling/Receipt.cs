using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities.Selling;

public class Receipt : BaseEntity
{
    [Required]
    [StringLength(30)]
    public string CreatedDate { get; set; } = string.Empty;
    [Required]
    public decimal TotalPrice { get; set; }
    [Required]
    public decimal Discount { get; set; }
    [Required]
    public decimal PaidCash { get; set; }
    [Required]
    public decimal PaidCard { get; set; }
    [Required]
    public bool HasLoan { get; set; }

    [Required]
    public string SellerId { get; set; } = string.Empty;


    public Loan Loan = new Loan();
    public IEnumerable<Transaction> Transactions = new List<Transaction>();
}
