using BLL.Dtos.CustomerDtos;
using BLL.Dtos.ProductDtos;
using BLL.Dtos.WarehouseItemDtos;
using DataLayer.Entities;
using DataLayer.Entities.Selling;

namespace BLL.Validations;

public static class Valide
{
    public static bool IsNullOrEmpty(this string value)
        => string.IsNullOrEmpty(value);

    public static bool IsEqual(this Product product, AddProductDto dto)
    {
        if (dto == null || product == null)
        {
            return false;
        }

        return dto.Name == product.Name &&
               dto.Color == product.Color &&
               dto.Size == product.Size &&
               dto.Brand == product.Brand &&
               dto.Barcode == product.Barcode;
    }

    public static bool IsEqual(this Customer customer, AddCustomerDto dto)
    {
        if (dto == null || customer == null)
        {
            return false;
        }

        return customer.FullName == dto.FullName &&
               customer.PhoneNumber == dto.PhoneNumber;
    }

    // product validation
    public static bool IsValid(this AddProductDto dto)
        =>      dto != null
            && !dto.Name.IsNullOrEmpty()
            && !dto.Color.IsNullOrEmpty()
            && !dto.Size.IsNullOrEmpty()
            && !dto.Brand.IsNullOrEmpty()
            && !dto.Barcode.IsNullOrEmpty()
            && dto.AdminId > 0;

    public static bool IsValid(this ProductUpdateDto dto)
       => dto != null
           && dto.Id > 0
           && !dto.Name.IsNullOrEmpty()
           && !dto.Color.IsNullOrEmpty()
           && !dto.Size.IsNullOrEmpty()
           && !dto.Brand.IsNullOrEmpty()
           && !dto.Barcode.IsNullOrEmpty();

    public static bool IsValid(this UpdateWarehouseItemDto dto)
        => dto != null
            && dto.Quantity > 0
            && dto.IncomingPrice > 0
            && dto.SellingPrice > 0
            && dto.AdminId > 0
            && dto.WarehouseId > 0
            && dto.ProductId > 0;

}