using BLL.Dtos.ProductDtos;
using DataLayer.Entities;

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

}