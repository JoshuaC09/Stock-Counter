using WebApplication2.Models;

namespace WebApplication2.Interfaces
{
    public interface IItemCount
    {
        Task AddItemCountAsync(
            string? itemCountCode,
            string? itemCode,
            string? itemDescription,
            string? itemUom,
            string? itemBatchLotNumber,
            string? itemExpiry,
            int? itemQuantity);
        Task DeleteItemCountAsync(string? itemKey);
        Task EditItemCountAsync(string? itemKey, string? itemBatchLotNumber, string? itemExpiry, int? itemQuantity);
        Task<IEnumerable<ItemCount>> ShowItemCountAsync(string countCode, int sort);
    }
}
