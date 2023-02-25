using DataLayer.Entities.Selling;

namespace BLL.Dtos.CustomerDtos;

public class CustomerViewDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;

    public static explicit operator CustomerViewDto(Customer v)
        => new CustomerViewDto()
        {
            Id = v.Id,
            FullName = v.FullName,
            PhoneNumber = v.PhoneNumber
        };

    public static explicit operator Customer(CustomerViewDto v)
        => new Customer()
        {
            Id = v.Id,
            FullName = v.FullName,
            PhoneNumber = v.PhoneNumber
        };
}