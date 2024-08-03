using WebApplication2.Helpers;
using WebApplication2.Interfaces;
using WebApplication2.Models;

public class InventoryService : IInventoryService
{
    private readonly IConnectionStringProviderService _connectionStringProvider;

    public InventoryService(IConnectionStringProviderService connectionStringProvider)
    {
        _connectionStringProvider = connectionStringProvider;
    }

    public async Task InitInventoryAsync()
    {
        using (var connection = await DatabaseHelper.GetOpenConnectionAsync(_connectionStringProvider))
        using (var command = DatabaseHelper.CreateCommand(connection, "init_inventory"))
        {
            await command.ExecuteNonQueryAsync();
        }
    }

    public async Task PostInventoryAsync()
    {
        using (var connection = await DatabaseHelper.GetOpenConnectionAsync(_connectionStringProvider))
        using (var command = DatabaseHelper.CreateCommand(connection, "post_inventory"))
        {
            await command.ExecuteNonQueryAsync();
        }
    }

    public async Task<List<ExportedItem>> ExportInventoryAsync()
    {
        var exportedItems = new List<ExportedItem>();

        using (var connection = await DatabaseHelper.GetOpenConnectionAsync(_connectionStringProvider))
        using (var command = DatabaseHelper.CreateCommand(connection, "for_export"))
        {
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var item = new ExportedItem
                    {
                        ItemNo = reader["Item No"].ToString(),
                        ItemUserDefine = reader["Item User Define"].ToString(),
                        Barcode = reader["Barcode"].ToString(),
                        Description = reader["Description"].ToString(),
                        BUOM = reader["BUOM"].ToString(),
                        Stocks = reader.IsDBNull(reader.GetOrdinal("Stocks(Pcs)")) ? (int?)null : reader.GetInt32("Stocks(Pcs)"),
                        LotNo = reader["Lot #"].ToString(),
                        Expiration = reader["Expiration"].ToString(),
                        Variance = reader.IsDBNull(reader.GetOrdinal("Variance")) ? (int?)null : reader.GetInt32("Variance"),
                        Rack = reader["Rack"].ToString(),
                        CFactor = reader["CFactor"].ToString(),
                        Counter = reader.IsDBNull(reader.GetOrdinal("Cntr")) ? (int?)null : reader.GetInt32("Cntr")
                    };
                    exportedItems.Add(item);
                }
            }
        }
        return exportedItems;
    }
}
