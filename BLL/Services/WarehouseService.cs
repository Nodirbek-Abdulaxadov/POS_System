using BLL.Dtos.ProductDtos;
using BLL.Dtos.WarehouseDtos;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Validations;
using DataLayer.Entities;
using DataLayer.Interfaces;

namespace BLL.Services;

public class WarehouseService : IWarehouseService
{
    private readonly IUnitOfWork _unitOfWork;

    public WarehouseService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Create new Warehouse
    /// </summary>
    /// <param name="dto">New warehouse name</param>
    /// <returns>New warehouse</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<WarehouseViewDto> AddAsync(AddWarehouseDto dto)
    {
        if (dto == null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

        if (string.IsNullOrEmpty(dto.Name))
        {
            throw new ArgumentNullException(nameof(dto.Name));
        }

        var list = await GetAllAsync();
        if (list.Any(x => x.Name == dto.Name))
        {
            throw new MarketException("This warehouse name is already exist!");
        }

        var model = await _unitOfWork.Warehouses.AddAsync((Warehouse)dto);
        await _unitOfWork.SaveAsync();

        return (WarehouseViewDto)model;
    }

    /// <summary>
    /// Get all warehouse list
    /// </summary>
    /// <returns>List of Warehouses</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<IEnumerable<WarehouseViewDto>> GetAllAsync()
    {
        var list = await _unitOfWork.Warehouses.GetAllAsync();

        if (list == null)
        {
            throw new ArgumentNullException(nameof(list));
        }

        var dtoList = list.Select(x => (WarehouseViewDto)x);
        return dtoList;
    }

    /// <summary>
    /// Get warehouses with pagination
    /// </summary>
    /// <param name="pageSize"></param>
    /// <param name="pageNumber"></param>
    /// <returns>Paged list of warehouses</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<PagedList<WarehouseViewDto>> GetWarehousesAsync(int pageSize, int pageNumber)
    {
        var list = await _unitOfWork.Warehouses.GetAllAsync();

        if (list == null)
        {
            throw new ArgumentNullException(nameof(list));
        }

        var dtoList = list.Select(x => (WarehouseViewDto)x);

        PagedList<WarehouseViewDto> pagedList = new (dtoList.ToList(),
                                                     dtoList.Count(),
                                                     pageSize, pageNumber);

        if (pageNumber > pagedList.TotalPages || pageNumber < 1)
        {
            throw new MarketException("Page not fount!");
        }

        return PagedList<WarehouseViewDto>.ToPagedList(dtoList, pageSize, pageNumber);
    }

    /// <summary>
    /// Get warehouse by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Single Warehouse view dto</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<WarehouseViewDto> GetByIdAsync(int id)
    {
        var warehouse = await _unitOfWork.Warehouses.GetByIdAsync(id);
        if (warehouse == null)
        {
            throw new ArgumentNullException(nameof(warehouse));
        }

        return (WarehouseViewDto)warehouse;
    }

    /// <summary>
    /// Remove warehouse model by id
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task RemoveAsync(int id)
    {
        var model = await _unitOfWork.Warehouses.GetByIdAsync(id);

        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        model.IsDeleted = true;
        await _unitOfWork.Warehouses.UpdateAsync(model);
        await _unitOfWork.SaveAsync();
    }

    /// <summary>
    /// Update warehouse with new name
    /// </summary>
    /// <param name="dto"></param>
    /// <returns>Updated warehuse model</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<WarehouseViewDto> UpdateAsync(WarehouseUpdateDto dto)
    {
        if (dto == null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

        if (string.IsNullOrEmpty(dto.Name))
        {
            throw new ArgumentNullException(nameof(dto.Name));
        }

        var model = await _unitOfWork.Warehouses.GetByIdAsync(dto.Id);

        if (model == null)
        {
            throw new MarketException("Warehouse is not found!");
        }

        model = (Warehouse)dto;
        await _unitOfWork.Warehouses.UpdateAsync(model);
        await _unitOfWork.SaveAsync();

        var res = await GetByIdAsync(dto.Id);
        return res;
    }
}
