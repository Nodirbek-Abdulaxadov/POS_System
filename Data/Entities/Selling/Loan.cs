using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities.Selling;

public class Loan : BaseEntity
{
    [Required]
    [StringLength(30)]
    public string GivenDate { get; set; } = string.Empty;
    [Required]
    public decimal PaidAmount { get; set; }
    [Required]
    public decimal LeftAmount { get; set; }

    [Required]
    public int OrderId { get; set; }
    [Required]
    public int CustomerId { get; set; }


    public IEnumerable<LoanPayment> LoanPayments = new List<LoanPayment>();
}
