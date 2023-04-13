using BLL.Dtos.WarehouseItemDtos;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Validations;
using Core;
using DataLayer.Entities;
using DataLayer.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BLL.Services;

public class WarehouseItemService : IWarehouseItemService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;

    public WarehouseItemService(IUnitOfWork unitOfWork,
                                UserManager<User> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
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

        var model = await _unitOfWork.WarehouseItems.AddAsync((WarehouseItem)dto);
        await _unitOfWork.SaveAsync();

        return (WarehouseItemDto)model;
    }

    public async Task<PagedList<WarehouseItemViewDto>> GetAllAsPaged()
    {
        var dtoList = (await _unitOfWork.WarehouseItems.GetAllAsync())
                                                   .Where(w => w.IsDeleted == false);

        var list = await Convert(dtoList);

        PagedList<WarehouseItemViewDto> pagedList = new(list.ToList(),
                                                     dtoList.Count(),
                                                     dtoList.Count(), 1);

        return pagedList.ToPagedList(list, dtoList.Count(), 1);
    }

    public async Task<IEnumerable<WarehouseItemViewDto>> GetAllAsync()
    {
        var list = (await _unitOfWork.WarehouseItems.GetAllAsync())
                            .Where(x => x.IsDeleted == false);

        if (list == null)
        {
            throw new ArgumentNullException(nameof(list));
        }

        return await Convert(list);
    }

    public async Task<PagedList<WarehouseItemViewDto>> GetArchivedAsync(int pageSize, int pageNumber)
    {
        var dtoList = (await _unitOfWork.WarehouseItems.GetAllAsync())
                                                   .Where(w => w.IsDeleted == true);

        var list = await Convert(dtoList);
        PagedList<WarehouseItemViewDto> pagedList = new(list.ToList(),
                                                     dtoList.Count(),
                                                     pageSize, pageNumber);

        if (pageNumber > pagedList.TotalPages || pageNumber < 1)
        {
            throw new MarketException("Page not fount!");
        }


        return pagedList.ToPagedList(list, pageSize, pageNumber);
    }

    public async Task<WarehouseItemViewDto> GetByIdAsync(int id)
    {
        var x = await _unitOfWork.WarehouseItems.GetByIdAsync(id);
        var products = await _unitOfWork.Products.GetAllAsync();
        var warehouses = await _unitOfWork.Warehouses.GetAllAsync();
        var admins = _userManager.Users.ToList();
        if (x == null)
        {
            throw new ArgumentNullException(nameof(x));
        }

        var product = products.FirstOrDefault(p => p.Id == x.ProductId);
        var warehouse = warehouses.FirstOrDefault(w => w.Id == x.WarehouseId);
        var admin = admins.FirstOrDefault(a => a.Id == x.AdminId);
        return new WarehouseItemViewDto()
        {
            Id = x.Id,
            AdminFullName = admin.FullName,
            AdminId = x.AdminId,
            BroughtDate = x.BroughtDate,
            IncomingPrice = x.IncomingPrice,
            SellingPrice = x.SellingPrice,
            ProductId = x.ProductId,
            ProductName = product.Name,
            Quantity = x.Quantity,
            WarehouseId = x.WarehouseId,
            WarehouseName = warehouse.Name
        };
    }

    public async Task<PagedList<WarehouseItemViewDto>> GetPagedAsync(int pageSize, int pageNumber, int warehouseId)
    {
        var dtoList = (await _unitOfWork.WarehouseItems.GetAllAsync())
                                                   .Where(w => w.IsDeleted == false && w.WarehouseId == warehouseId);

        var list = await Convert(dtoList);
        PagedList<WarehouseItemViewDto> pagedList = new(list.ToList(),
                                                     dtoList.Count(),
                                                     pageSize, pageNumber);

        if (pagedList.Data.Count == 0)
        {
            throw new MarketException("Empty list");
        }

        if (pageNumber > pagedList.TotalPages || pageNumber < 1)
        {
            throw new ArgumentNullException("Page not fount!");
        }


        return pagedList.ToPagedList(list, pageSize, pageNumber);
    }

    public async Task<WarehouseItemViewDto> Update(UpdateWarehouseItemDto dto)
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

    private async Task<IEnumerable<WarehouseItemViewDto>> Convert(IEnumerable<WarehouseItem> list)
    {
        var products = await _unitOfWork.Products.GetAllAsync();
        var warehouses = await _unitOfWork.Warehouses.GetAllAsync();
        var admins = _userManager.Users.ToList();

        var dtoList = list.Select(x =>
                          {
                              var product = products.FirstOrDefault(p => p.Id == x.ProductId)??(new Product() { Name = ""});
                              var warehouse = warehouses.FirstOrDefault(w => w.Id == x.WarehouseId) ?? (new Warehouse() { Name = "" }); ;
                              var admin = admins.FirstOrDefault(a => a.Id == x.AdminId) ?? (new User() { FullName = "" }); ;
                              return new WarehouseItemViewDto()
                              {
                                  Id = x.Id,
                                  AdminFullName = admin.FullName,
                                  AdminId = x.AdminId,
                                  BroughtDate = x.BroughtDate,
                                  IncomingPrice = x.IncomingPrice,
                                  SellingPrice = x.SellingPrice,
                                  ProductId = x.ProductId,
                                  ProductName = product.Name,
                                  Quantity = x.Quantity,
                                  WarehouseId = x.WarehouseId,
                                  WarehouseName = warehouse.Name
                              };
                          });
        return dtoList;
    }
}