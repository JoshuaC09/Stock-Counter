using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Interfaces;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;

    public InventoryController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    [HttpPost("init")]
    public async Task<IActionResult> InitInventory()
    {
        await _inventoryService.InitInventoryAsync();
        return Ok();
    }

    [HttpPost("post")]
    public async Task<IActionResult> PostInventory()
    {
        await _inventoryService.PostInventoryAsync();
        return Ok();
    }

    [HttpPost("export")]
    public async Task<IActionResult> ExportInventory()
    {
        var exportedItems = await _inventoryService.ExportInventoryAsync();
        return Ok(exportedItems);
    }
}
