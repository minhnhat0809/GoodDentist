using System.Linq.Expressions;
using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace Repositories;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected GoodDentistDbContext _repositoryContext;
    public RepositoryBase(GoodDentistDbContext repositoryContext)
    {
        _repositoryContext = repositoryContext;
    }
    public Task<List<T>> FindAllAsync() => _repositoryContext.Set<T>().AsNoTracking().ToListAsync();
    public Task<List<T>> FindByConditionAsync(Expression<Func<T, bool>> expression) =>
        _repositoryContext.Set<T>().Where(expression).AsNoTracking().ToListAsync();


    public async Task<bool> CreateAsync(T entity)
    {
        try
        {
            await _repositoryContext.Set<T>().AddAsync(entity);
            return await SaveChange();
        } catch (Exception ex)
        {
            throw new Exception("Fail to add",ex);
        }
    }
    public async Task<bool> UpdateAsync(T entity)
    {
        try
        {
            _repositoryContext.Set<T>().Update(entity);
            return await SaveChange();
        }
        catch (Exception ex)
        {
            throw new Exception("Fail to update", ex);
        }
    }
    public async Task<bool> DeleteAsync(T entity)
    {
        try
        {
            _repositoryContext.Set<T>().Update(entity);
            return await SaveChange();
        }
        catch (Exception ex)
        {
            throw new Exception("Fail to remove", ex);
        }
    }
    public async Task<bool> SaveChange()
    {
        var result = await _repositoryContext.SaveChangesAsync();
        return result > 0;
    }

}