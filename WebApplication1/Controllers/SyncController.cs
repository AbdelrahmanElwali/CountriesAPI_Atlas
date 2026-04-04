using CountriesAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CountriesAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SyncController : ControllerBase
{
    private readonly ISyncService _syncService;

    public SyncController(ISyncService syncService)
    {
        _syncService = syncService;
    }

    [HttpPost]
    public async Task<IActionResult> Sync()
    {
        await _syncService.SyncAsync();
        return Ok("Sync completed successfully!");
    }

    [HttpPost("translations")]
    public async Task<IActionResult> SyncTranslations()
    {
        await _syncService.SyncTranslationsAsync();
        return Ok("Translations synced successfully!");
    }
}