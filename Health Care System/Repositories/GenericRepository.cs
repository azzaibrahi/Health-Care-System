using Health_Care_System.Data;
using Health_Care_System.Entities;
using Microsoft.EntityFrameworkCore;

namespace Health_Care_System.Repositories;

/// <summary>
/// One implementation for every <see cref="BaseEntity"/>; EF Core resolves
/// the table from the closed generic type, so no per-entity repository is needed.
/// </summary>
/// <typeparam name="T">Entity type.</typeparam>
public class GenericRepository<T>(ApplicationDbContext dbContext) : IGenericRepository<T>
    where T : BaseEntity
{
    public async Task<IEnumerable<T>> GetAllAsync()
        => await dbContext.Set<T>().ToListAsync();

    public async Task<T?> GetByIdAsync(int id)
        => await dbContext.Set<T>().FindAsync(id);

    public async Task<T> CreateAsync(T entity)
    {
        await dbContext.Set<T>().AddAsync(entity);
        await dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(T entity)
    {
        var model = await GetByIdAsync(entity.Id);

        if (model is null)
        {
            return false;
        }

        dbContext.Set<T>().Update(entity);
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);

        if (entity is null)
        {
            return false;
        }

        // SoftDeleteInterceptor turns this physical delete into IsDeleted = true.
        dbContext.Set<T>().Remove(entity);
        await dbContext.SaveChangesAsync();
        return true;
    }
}
