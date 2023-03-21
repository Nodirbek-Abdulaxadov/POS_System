using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities;

public class WarehouseItem : BaseEntity
{
    [Required]
    public int Quantity { get; set; }
    [Required]
    [StringLength(30)]
    public string BroughtDate { get; set; } = string.Empty;
    [Required]
    public decimal IncomingPrice { get; set; }
    [Required]
    public decimal SellingPrice { get; set;}

    [Required]
    public int ProductId { get; set; }
    [Required]
    public string AdminId { get; set; } = string.Empty;
    [Required]
    public int WarehouseId { get; set; }
}
