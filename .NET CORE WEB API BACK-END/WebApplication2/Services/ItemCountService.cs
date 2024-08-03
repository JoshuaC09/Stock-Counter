using WebApplication2.Helpers;
using WebApplication2.Interfaces;
using WebApplication2.Models;

namespace WebApplication2.Services
{
    public class ItemCountService : IItemCount
    {
        private readonly IConnectionStringProviderService _connectionStringProvider;

        public ItemCountService(IConnectionStringProviderService connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public async Task AddItemCountAsync(
            string? itemCountCode,
            string? itemCode,
            string? itemDescription,
            string? itemUom,
            string? itemBatchLotNumber,
            string? itemExpiry,
            int? itemQuantity)
        {
            using (var connection = await DatabaseHelper.GetOpenConnectionAsync(_connectionStringProvider))
            using (var command = DatabaseHelper.CreateCommand(connection, "add_item_count",
                ("_cntcode", itemCountCode ?? ""),
                ("_itmcode", itemCode ?? ""),
                ("_itmdesc", itemDescription ?? ""),
                ("_itmuom", itemUom ?? ""),
                ("_itmbnl", itemBatchLotNumber ?? ""),
                ("_itmexp", itemExpiry ?? ""),
                ("_itmqty", itemQuantity ?? 0)))
            {
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteItemCountAsync(string itemKey)
        {
            // This method signature doesn't need to change
            using (var connection = await DatabaseHelper.GetOpenConnectionAsync(_connectionStringProvider))
            using (var command = DatabaseHelper.CreateCommand(connection, "del_item_count", ("_itmkey", itemKey)))
            {
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task EditItemCountAsync(string? itemKey, string? itemBatchLotNumber, string? itemExpiry, int? itemQuantity)
        {
            using (var connection = await DatabaseHelper.GetOpenConnectionAsync(_connectionStringProvider))
            using (var command = DatabaseHelper.CreateCommand(connection, "edit_item_count",
                ("_itmkey", itemKey ?? ""),
                ("_itmbnl", itemBatchLotNumber ?? ""),
                ("_itmexp", itemExpiry ?? ""),
                ("_itmqty", itemQuantity ?? 0)))
            {
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<IEnumerable<ItemCount>> ShowItemCountAsync(string countCode, int sort)
        {
            var itemCounts = new List<ItemCount>();
            using (var connection = await DatabaseHelper.GetOpenConnectionAsync(_connectionStringProvider))
            using (var command = DatabaseHelper.CreateCommand(connection, "show_item_count",
                ("_cntcode", countCode),
                ("_sort", sort)))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    itemCounts.Add(new ItemCount
                    {
                        ItemKey = reader.GetString("itm_key"),
                        ItemCounter = reader.GetInt32("itm_ctr"),
                        ItemCode = reader.GetString("itm_code"),
                        ItemDescription = reader.GetString("itm_description"),
                        ItemUom = reader.GetString("itm_uom"),
                        ItemBatchLotNumber = reader.GetString("itm_batchlot"),
                        ItemExpiry = reader.GetString("itm_expiry"),
                        ItemQuantity = reader.GetInt32("itm_quantity"),
                    });
                }
            }
            return itemCounts;
        }
    }
}