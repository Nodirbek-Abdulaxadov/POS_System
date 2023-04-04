using BLL.Dtos.WarehouseItemDtos;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Validations;
using DataLayer.Entities;
using DataLayer.Interfaces;

namespace BLL.Services;

public class WarehouseItemService : IWarehouseItemService
{
    private readonly IUnitOfWork _unitOfWork;

    public WarehouseItemService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task ActionAsync(int id, ActionType action)
    {
        var model = await _unitOfWork.WarehouseItems.GetByIdAsync(id);

        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        switch (action)
        {
            case ActionType.Archive:
                {
                    model.IsDeleted = true;

                    await _unitOfWork.WarehouseItems.UpdateAsync(model);
                }
                break;
            case ActionType.Recover:
                {
                    model.IsDeleted = false;
                    await _unitOfWork.WarehouseItems.UpdateAsync(model);
                }
                break;
            case ActionType.Remove:
                {
                    await _unitOfWork.WarehouseItems.RemoveAsync(model);
                }
                break;
        }

        await _unitOfWork.SaveAsync();
    }

    /// <summary>
    /// Create new WarehouseItem
    /// </summary>
    /// <param name="dto"></param>
    /// <returns>New Warehouse item</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="MarketException"></exception>
    public async Task<WarehouseItemDto> AddAsync(AddWarehouseItemDto dto)
    {
        if (dto == null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

        if (dto.IncomingPrice <= 0 || dto.SellingPrice <= 0 || dto.Quantity <= 0)
        {
            throw new MarketException("All fields must be positive numbers!");
        }

        var list = await GetAllAsync(dto.WarehouseId);

        var model = await _unitOfWork.WarehouseItems.AddAsync((WarehouseItem)dto);
        await _unitOfWork.SaveAsync();

        return (WarehouseItemDto)model;
    }

    public async Task<IEnumerable<WarehouseItemDto>> GetAllAsync(int warehouseId)
    {
        var list = await _unitOfWork.WarehouseItems.GetAllAsync();

        if (list == null)
        {
            throw new ArgumentNullException(nameof(list));
        }

        var dtoList = list.Where(w => w.WarehouseId == warehouseId)
                          .Select(x => (WarehouseItemDto)x);
        return dtoList;
    }

    public async Task<PagedList<WarehouseItemDto>> GetArchivedAsync(int pageSize, int pageNumber)
    {
        var dtoList = (await _unitOfWork.WarehouseItems.GetAllAsync())
                                                   .Where(w => w.IsDeleted == true)
                                                   .Select(i => (WarehouseItemDto)i)
                                                   .ToList();

        PagedList<WarehouseItemDto> pagedList = new(dtoList.ToList(),
                                                     dtoList.Count(),
                                                     pageSize, pageNumber);

        if (pageNumber > pagedList.TotalPages || pageNumber < 1)
        {
            throw new MarketException("Page not fount!");
        }

        return pagedList.ToPagedList(dtoList, pageSize, pageNumber);
    }

    public async Task<WarehouseItemDto> GetByIdAsync(int id)
    {
        var warehouseItem = await _unitOfWork.WarehouseItems.GetByIdAsync(id);
        if (warehouseItem == null)
        {
            throw new ArgumentNullException(nameof(warehouseItem));
        }

        return (WarehouseItemDto)warehouseItem;
    }

    public async Task<PagedList<WarehouseItemDto>> GetPagedAsync(int pageSize, int pageNumber, int warehouseId)
    {
        var dtoList = (await _unitOfWork.WarehouseItems.GetAllAsync())
                                                   .Where(w => w.IsDeleted == false && w.WarehouseId == warehouseId)
                                                   .Select(i => (WarehouseItemDto)i)
                                                   .ToList();

        PagedList<WarehouseItemDto> pagedList = new(dtoList.ToList(),
                                                     dtoList.Count(),
                                                     pageSize, pageNumber);

        if (pageNumber > pagedList.TotalPages || pageNumber < 1)
        {
            throw new MarketException("Page not fount!");
        }

        return pagedList.ToPagedList(dtoList, pageSize, pageNumber);
    }

    public async Task<WarehouseItemDto> Update(UpdateWarehouseItemDto dto)
    {
        if (!dto.IsValid() || dto == null)
        {
            throw new MarketException("Invalid model!");
        }

        var model = await _unitOfWork.WarehouseItems.GetByIdAsync(dto.Id);
        if (model == null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

        await _unitOfWork.WarehouseItems.UpdateAsync((WarehouseItem)dto);
        await _unitOfWork.SaveAsync();

        return await GetByIdAsync(model.Id);
    }
}