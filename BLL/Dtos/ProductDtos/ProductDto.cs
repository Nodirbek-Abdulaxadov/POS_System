using DataLayer.Entities;

namespace BLL.Dtos.ProductDtos
{
    public class ProductDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public string Barcode { get; set; } = string.Empty;
        public int WarningCount { get; set; }

        public int AdminId { get; set; }
        public string AddedDate { get; set; } = string.Empty;

        public IEnumerable<WarehouseItem> Products = new List<WarehouseItem>();
    }
}
