using WebApplication2.Helpers;
using WebApplication2.Interfaces;
using WebApplication2.Models;

namespace WebApplication2.Services
{
    public class ItemService : IItemService
    {
        private readonly IConnectionStringProviderService _connectionStringProvider;

        public ItemService(IConnectionStringProviderService connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(string databaseName, string? pattern)
        {
            var items = new List<Item>();
            string remoteDatabase = await _connectionStringProvider.GetRemoteDatabaseAsync() ?? databaseName;

            using (var connection = await DatabaseHelper.GetOpenConnectionAsync(_connectionStringProvider))
            using (var command = DatabaseHelper.CreateCommand(connection, "getinv", ("rmschma", remoteDatabase), ("patrn", pattern ?? string.Empty)))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    items.Add(new Item
                    {
                        ItemNumber = reader.GetString("itemno"),
                        ItemDescription = reader.GetString("itemdesc"),
                        SellingUom = reader.GetString("suom")
                    });
                }
            }
            return items;
        }
    }
}
