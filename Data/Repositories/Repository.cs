using DataLayer.Context;
using DataLayer.Entities;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories;

public class Repository<TEntity>
        : IRepository<TEntity> where TEntity : BaseEntity
{
    private readonly AppDbContext _dbContext;

    public Repository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await _dbContext.Set<TEntity>().AddAsync(entity);
        return entity;
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
        => await _dbContext.Set<TEntity>()
                           .AsNoTracking()
                           .ToListAsync();

    public async Task<TEntity?> GetByIdAsync(int id)
        => await _dbContext.Set<TEntity>()
                           .AsNoTracking() 
                           .FirstOrDefaultAsync(x => x.Id == id);

    public Task RemoveAsync(TEntity entity)
    {
        _dbContext.Set<TEntity>().Remove(entity);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(TEntity entity)
    {
        _dbContext.Set<TEntity>().Update(entity);
        return Task.FromResult(entity);
    }
}