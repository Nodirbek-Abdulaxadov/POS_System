using BLL.Dtos.CategoryDtos;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Validations;
using DataLayer.Entities;
using DataLayer.Interfaces;

namespace BLL.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task ActionAsync(int id, ActionType action)
    {
        var model = await _unitOfWork.Categories.GetByIdAsync(id);

        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        switch (action)
        {
            case ActionType.Archive:
                {
                    model.IsDeleted = true;

                    await _unitOfWork.Categories.UpdateAsync(model);
                }
                break;
            case ActionType.Recover:
                {
                    model.IsDeleted = false;
                    await _unitOfWork.Categories.UpdateAsync(model);
                }
                break;
            case ActionType.Remove:
                {
                    await _unitOfWork.Categories.RemoveAsync(model);
                }
                break;
        }

        await _unitOfWork.SaveAsync();
    }

    public async Task<CategoryViewDto> AddAsync(AddCategoryDto dto)
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
            throw new MarketException("This category name is already exist!");
        }

        var model = await _unitOfWork.Categories.AddAsync((Category)dto);
        await _unitOfWork.SaveAsync();

        return (CategoryViewDto)model;
    }

    public async Task<IEnumerable<CategoryViewDto>> GetAllAsync()
    {
        var list = await _unitOfWork.Categories.GetAllAsync();

        if (list == null)
        {
            throw new ArgumentNullException(nameof(list));
        }

        var dtoList = list.Select(x => (CategoryViewDto)x);
        return dtoList;
    }

    public async Task<PagedList<CategoryViewDto>> GetArchivedCategoriesAsync(int pageSize, int pageNumber)
    {
        var dtoList = (await _unitOfWork.Categories.GetAllAsync())
                                                   .Where(w => w.IsDeleted == true)
                                                   .Select(i => (CategoryViewDto)i)
                                                   .ToList();

        PagedList<CategoryViewDto> pagedList = new(dtoList.ToList(),
                                                     dtoList.Count(),
                                                     pageSize, pageNumber);

        if (pageNumber > pagedList.TotalPages || pageNumber < 1)
        {
            throw new MarketException("Page not fount!");
        }

        return pagedList.ToPagedList(dtoList, pageSize, pageNumber);
    }

    public async Task<CategoryViewDto> GetByIdAsync(int id)
    {
        var warehouse = await _unitOfWork.Categories.GetByIdAsync(id);
        if (warehouse == null)
        {
            throw new ArgumentNullException(nameof(warehouse));
        }

        return (CategoryViewDto)warehouse;
    }

    public async Task<PagedList<CategoryViewDto>> GetCategoriesAsync(int pageSize, int pageNumber)
    {
        var dtoList = (await _unitOfWork.Categories.GetAllAsync())
                                                   .Where(w => w.IsDeleted == false)
                                                   .Select(i => (CategoryViewDto)i)
                                                   .ToList();

        PagedList<CategoryViewDto> pagedList = new(dtoList.ToList(),
                                                     dtoList.Count(),
                                                     pageSize, pageNumber);

        if (pageNumber > pagedList.TotalPages || pageNumber < 1)
        {
            throw new MarketException("Page not fount!");
        }

        return pagedList.ToPagedList(dtoList, pageSize, pageNumber);
    }

    public async Task<CategoryViewDto> UpdateAsync(UpdateCategoryDto dto)
    {
        if (dto == null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

        if (string.IsNullOrEmpty(dto.Name))
        {
            throw new ArgumentNullException(nameof(dto.Name));
        }

        var model = await _unitOfWork.Categories.GetByIdAsync(dto.Id);

        if (model == null)
        {
            throw new MarketException("Warehouse is not found!");
        }

        model = (Category)dto;
        await _unitOfWork.Categories.UpdateAsync(model);
        await _unitOfWork.SaveAsync();

        var res = await GetByIdAsync(dto.Id);
        return res;
    }
}