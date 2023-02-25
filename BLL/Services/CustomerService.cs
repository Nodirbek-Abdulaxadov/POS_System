using BLL.Dtos.CustomerDtos;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Validations;
using DataLayer.Entities.Selling;
using DataLayer.Interfaces;

namespace BLL.Services;

public class CustomerService : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork;

    public CustomerService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CustomerViewDto> AddAsync(AddCustomerDto dto)
    {
        if (dto == null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

        if (string.IsNullOrEmpty(dto.FullName) || string.IsNullOrEmpty(dto.PhoneNumber))
        {
            throw new MarketException("All fields must be non empty value");
        }

        var list = await _unitOfWork.Customers.GetAllAsync();
        if (list.Any(c => c.IsEqual(dto)))
        {
            throw new MarketException("This customer already exist!");
        }

        var model = await _unitOfWork.Customers.AddAsync((Customer)dto);
        await _unitOfWork.SaveAsync();

        return (CustomerViewDto)model;
    }

    public async Task<IEnumerable<CustomerViewDto>> GetAllAsync()
    {
        var list = await _unitOfWork.Customers.GetAllAsync();
        if (!list.Any())
        {
            throw new ArgumentNullException(nameof(list));
        }

        return list.Select(i => (CustomerViewDto)i);
    }

    public async Task<CustomerViewDto> GetByIdAsync(int id)
    {
        var model = await _unitOfWork.Customers.GetByIdAsync(id);

        if (model == null)
        {
            throw new ArgumentNullException($"{nameof(model)} is null");
        }

        return (CustomerViewDto)model;
    }

    public async Task<PagedList<CustomerViewDto>> GetPagedAsync(int pageSize, int pageNumber)
    {
        var dtoList = await GetAllAsync();
        PagedList<CustomerViewDto> pagedList = new(dtoList.ToList(),
                                                     dtoList.Count(),
                                                     pageSize, pageNumber);

        if (pageNumber > pagedList.TotalPages || pageNumber < 1)
        {
            throw new MarketException("Page not fount!");
        }

        return PagedList<CustomerViewDto>.ToPagedList(dtoList, pageSize, pageNumber);
    }

    public async Task RemoveAsync(int id)
    {
        var model = await _unitOfWork.Customers.GetByIdAsync(id);

        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        model.IsDeleted = true;
        await _unitOfWork.Customers.UpdateAsync(model);
        await _unitOfWork.SaveAsync();
    }

    public async Task<CustomerViewDto> UpdateAsync(CustomerViewDto dto)
    {
        if (dto == null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

        if (string.IsNullOrEmpty(dto.FullName) || string.IsNullOrEmpty(dto.PhoneNumber))
        {
            throw new ArgumentNullException();
        }

        var model = await _unitOfWork.Customers.GetByIdAsync(dto.Id);

        if (model == null)
        {
            throw new MarketException("Warehouse is not found!");
        }

        model = (Customer)dto;
        await _unitOfWork.Customers.UpdateAsync(model);
        await _unitOfWork.SaveAsync();

        var res = await GetByIdAsync(dto.Id);
        return res;
    }
}
