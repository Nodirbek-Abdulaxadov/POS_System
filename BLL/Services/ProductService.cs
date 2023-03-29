using BLL.Dtos.ProductDtos;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Validations;
using DataLayer.Entities;
using DataLayer.Interfaces;

namespace BLL.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task ActionAsync(int id, ActionType action)
    {
        var model = await _unitOfWork.Products.GetByIdAsync(id);

        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        switch (action)
        {
            case ActionType.Archive:
                {
                    model.IsDeleted = true;

                    await _unitOfWork.Products.UpdateAsync(model);
                }
                break;
            case ActionType.Recover:
                {
                    model.IsDeleted = false;
                    await _unitOfWork.Products.UpdateAsync(model);
                }
                break;
            case ActionType.Remove:
                {
                    await _unitOfWork.Products.RemoveAsync(model);
                }
                break;
        }

        await _unitOfWork.SaveAsync();
    }

    /// <summary>
    /// Add new product method
    /// </summary>
    /// <param name="dto"></param>
    /// <returns>New product model</returns>
    /// <exception cref="MarketException"></exception>
    public async Task<ProductViewDto> AddAsync(AddProductDto dto)
    {
        if (!dto.IsValid())
        {
            throw new MarketException("All fields must be non empty value!");
        }

        var products = await _unitOfWork.Products.GetAllAsync();
        var exist = products.Where(x => x.Name == dto.Name)
                            .Any(p => p.IsEqual(dto));

        if (exist)
        {
            throw new MarketException("This product is already exist!");
        }

        var model = await _unitOfWork.Products.AddAsync((Product)dto);
        await _unitOfWork.SaveAsync();

        return (ProductViewDto)model;
    }

    /// <summary>
    /// Get all products method
    /// </summary>
    /// <returns>List of products</returns>
    public async Task<IEnumerable<ProductViewDto>> GetAllAsync()
    {
        var list = await _unitOfWork.Products.GetAllAsync();

        if (list == null)
        {
            throw new ArgumentNullException(nameof(list));
        }

        var dtoList = list.Select(x =>
        {
            var model = (ProductViewDto)x;
            var category = _unitOfWork.Categories.GetByIdAsync(model.Id);
            if (category.Result != null)
            {
                model.CategoryName = category.Result.Name;
            }
            else
            {
                model.CategoryName = "Noma'lum";
            }

            return model;
        });
        return dtoList;
    }

    public async Task<PagedList<ProductViewDto>> GetArchivedProductsAsync(int pageSize, int pageNumber)
    {
        var dtoList = (await _unitOfWork.Products.GetAllAsync())
                                                   .Where(w => w.IsDeleted == true)
                                                   .Select(i =>
                                                   {
                                                       var model = (ProductViewDto)i;
                                                       var category = _unitOfWork.Categories.GetByIdAsync(model.Id);
                                                       if (category.Result != null)
                                                       {
                                                           model.CategoryName = category.Result.Name;
                                                       }
                                                       else
                                                       {
                                                           model.CategoryName = "Noma'lum";
                                                       }

                                                       return model;
                                                   })
                                                   .ToList();

        PagedList<ProductViewDto> pagedList = new(dtoList.ToList(),
                                                     dtoList.Count(),
                                                     pageSize, pageNumber);

        if (pageNumber > pagedList.TotalPages || pageNumber < 1)
        {
            throw new MarketException("Page not fount!");
        }

        return pagedList.ToPagedList(dtoList, pageSize, pageNumber);
    }

    /// <summary>
    /// Get single product by id method
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Product model</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="MarketException"></exception>
    public async Task<ProductViewDto> GetByIdAsync(int id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);

        if (product == null)
        {
            throw new ArgumentNullException(nameof(product));
        }

        var model = (ProductViewDto)product;
        var category = _unitOfWork.Categories.GetByIdAsync(model.Id);
        if (category.Result != null)
        {
            model.CategoryName = category.Result.Name;
        }
        else
        {
            model.CategoryName = "Noma'lum";
        }

        return model;
    }

    public async Task<List<DProduct>> GetDProducts()
    {
        var products = await _unitOfWork.Products.GetAllAsync();
        var warehouseItems = await _unitOfWork.WarehouseItems.GetAllAsync();
        List<DProduct> dProducts = new List<DProduct>();
        foreach (var product in products)
        {
            var productItems = warehouseItems.Where(d => d.ProductId == product.Id);
            var warItem = productItems.First();
            var model = new DProduct()
            {
                Id = product.Id,
                Name = product.Name,
                Barcode = product.Barcode,
                Brand = product.Brand,
                Color = product.Color,
                Size = product.Size,
                Price = warItem.SellingPrice,
                AvailableCount = productItems.Sum(d => d.Quantity)
            };
            dProducts.Add(model);
        }

        return dProducts;
    }

    /// <summary>
    /// Get paged list of products
    /// </summary>
    /// <param name="pageSize"></param>
    /// <param name="pageNumber"></param>
    /// <returns>Paged list</returns>
    public async Task<PagedList<ProductViewDto>> GetProductsAsync(int pageSize, int pageNumber)
    {
        var dtoList = (await _unitOfWork.Products.GetAllAsync())
                                                   .Where(w => w.IsDeleted == false)
                                                   .Select(i =>
                                                   {
                                                       var model = (ProductViewDto)i;
                                                       var category = _unitOfWork.Categories.GetByIdAsync(model.Id);
                                                       if (category.Result != null)
                                                       {
                                                           model.CategoryName = category.Result.Name;
                                                       }
                                                       else
                                                       {
                                                           model.CategoryName = "Noma'lum";
                                                       }

                                                       return model;
                                                   })
                                                   .ToList();

        PagedList<ProductViewDto> pagedList = new(dtoList.ToList(),
                                                     dtoList.Count(),
                                                     pageSize, pageNumber);

        if (pageNumber > pagedList.TotalPages || pageNumber < 1)
        {
            throw new MarketException("Page not fount!");
        }

        return pagedList.ToPagedList(dtoList, pageSize, pageNumber);
    }

    /// <summary>
    /// Update product method
    /// </summary>
    /// <param name="dto"></param>
    /// <returns>Updated model</returns>
    /// <exception cref="MarketException"></exception>
    public async Task<ProductViewDto> UpdateAsync(ProductUpdateDto dto)
    {
        if (!dto.IsValid())
        {
            throw new MarketException("All fields must be non empty value!");
        }

        var model = await _unitOfWork.Products.GetByIdAsync(dto.Id);

        if (model == null)
        {
            throw new MarketException("Product is not found!");
        }

        await _unitOfWork.Products.UpdateAsync((Product)dto);
        await _unitOfWork.SaveAsync();

        return (ProductViewDto)model;
    }
}