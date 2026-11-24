using Health_Care_System.Entities;

namespace Health_Care_System.Repositories;

/// <summary>
/// CRUD contract shared by every entity that inherits <see cref="BaseEntity"/>
/// and therefore has an <c>Id</c> primary key.
/// </summary>
/// <typeparam name="T">Entity type.</typeparam>
public interface IGenericRepository<T> where T : BaseEntity
{
    Task<IEnumerable<T>> GetAllAsync();

    Task<T?> GetByIdAsync(int id);

    Task<T> CreateAsync(T entity);

    Task<bool> UpdateAsync(T entity);

    Task<bool> DeleteAsync(int id);
}
