using SQLite; // Still needed for [PrimaryKey], [Indexed], etc. in entity models
using ConectaBairro.Models.Entities; // Assuming T will often be an entity

namespace ConectaBairro.Services;

public class Repository<T> : IRepository<T> where T : class, new()
{
    protected readonly DatabaseService _databaseService;

    public Repository()
    {
        _databaseService = DatabaseService.Instance.Result; // Synchronously get instance for constructor
    }

    public virtual async Task<List<T>> GetAllAsync()
    {
        return await _databaseService.GetItemsAsync<T>();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await _databaseService.GetItemByGuidAsync<T>(id);
    }
    
    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _databaseService.GetItemAsync<T>(id);
    }

    public virtual async Task<int> InsertAsync(T entity)
    {
        return await _databaseService.SaveItemAsync(entity);
    }

    public virtual async Task<int> UpdateAsync(T entity)
    {
        return await _databaseService.SaveItemAsync(entity); // SaveItemAsync handles update
    }

    public virtual async Task<int> DeleteAsync(T entity)
    {
        return await _databaseService.DeleteItemAsync(entity);
    }

    // DeleteByIdAsync removed as SaveItemAsync is generic. Specific repos can implement if needed.
}
