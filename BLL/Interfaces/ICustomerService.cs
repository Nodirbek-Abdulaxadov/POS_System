using BLL.Dtos.CustomerDtos;
using BLL.Helpers;

namespace BLL.Interfaces;

public interface ICustomerService
{
    Task<PagedList<CustomerViewDto>> GetPagedAsync(int pageSize, int pageNumber);
    Task<IEnumerable<CustomerViewDto>> GetAllAsync();

    Task<CustomerViewDto> GetByIdAsync(int id);
    Task<CustomerViewDto> AddAsync(AddCustomerDto dto);

    Task<CustomerViewDto> UpdateAsync(CustomerViewDto dto);
    Task RemoveAsync(int id);
}
