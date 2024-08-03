using WebApplication2.Models;

namespace WebApplication2.Interfaces
{
    public interface IItemService
    {
        Task<IEnumerable<Item>> GetItemsAsync(string databaseName, string? pattern);
    }
}
