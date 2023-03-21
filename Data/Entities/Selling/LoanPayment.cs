using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities.Selling;

public class LoanPayment : BaseEntity
{
    [Required]
    [StringLength(30)]
    public string PaidDate { get; set; } = string.Empty;
    [Required]
    public decimal PaidAmount { get; set; }


    public int LoanId { get; set; }
    public string SellerId { get; set; } = string.Empty;
}