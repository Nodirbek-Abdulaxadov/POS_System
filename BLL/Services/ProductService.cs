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
        var products = await _unitOfWork.Products.GetAllAsync();
        return products.Where(x => x.IsDeleted == false)
                       .Select(p => (ProductViewDto)p);
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

        return (ProductViewDto)product;
    }

    /// <summary>
    /// Get paged list of products
    /// </summary>
    /// <param name="pageSize"></param>
    /// <param name="pageNumber"></param>
    /// <returns>Paged list</returns>
    public async Task<PagedList<ProductViewDto>> GetProductsAsync(int pageSize, int pageNumber)
    {
        var list = await GetAllAsync();

        PagedList<ProductViewDto> pagedList = new ( list.ToList(), 
                                                    list.Count(), 
                                                    pageSize, pageNumber);

        if (pageNumber > pagedList.TotalPages || pageNumber < 1)
        {
            throw new MarketException("Page not fount!");
        }

        return PagedList<ProductViewDto>.ToPagedList(list, pageSize, pageNumber);
    }

    /// <summary>
    /// Remove product by id method
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task RemoveAsync(int id)
    {
        var model = await _unitOfWork.Products.GetByIdAsync(id);

        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        model.IsDeleted = true;
        await _unitOfWork.Products.UpdateAsync(model);
        await _unitOfWork.SaveAsync();
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