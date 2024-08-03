using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Interfaces;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ItemCountController : ControllerBase
    {
        private readonly IItemCount _itemCountService;

        public ItemCountController(IItemCount itemCountService)
        {
            _itemCountService = itemCountService;
        }
        [HttpPost("add")]
        public async Task<IActionResult> AddItemCount([FromBody] ItemCount itemCount)
        {
            await _itemCountService.AddItemCountAsync(
                itemCount.ItemCountCode,
                itemCount.ItemCode,
                itemCount.ItemDescription,
                itemCount.ItemUom,
                itemCount.ItemBatchLotNumber,
                itemCount.ItemExpiry,
                itemCount.ItemQuantity ?? 0);  
            return Ok();
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteItemCount(string itemKey)
        {
            await _itemCountService.DeleteItemCountAsync(itemKey);
            return Ok();
        }

        [HttpPut("edit")]
        public async Task<IActionResult> EditItemCount([FromBody] ItemCount itemCount)
        {
            await _itemCountService.EditItemCountAsync(
                itemCount.ItemKey,
                itemCount.ItemBatchLotNumber,
                itemCount.ItemExpiry,
                itemCount.ItemQuantity ?? 0); 
            return Ok();
        }

        [HttpGet("show")]
        public async Task<IEnumerable<ItemCount>> ShowItemCount(string countCode, int sort)
        {
            return await _itemCountService.ShowItemCountAsync(countCode, sort);
        }
    }
}
