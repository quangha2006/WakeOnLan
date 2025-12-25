using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WakeOnLan.WebApp.Services;

namespace WakeOnLan.WebApp.Controllers;

[ApiController]
[Route("api/wol")]
[Authorize]
public sealed class WolController : ControllerBase
{
    private readonly IComputerCatalog _catalog;
    private readonly IWakeOnLanService _wol;
    private readonly ILogger<WolController> _logger;

    public WolController(IComputerCatalog catalog, IWakeOnLanService wol, ILogger<WolController> logger)
    {
        _catalog = catalog;
        _wol = wol;
        _logger = logger;
    }

    public sealed class WakeByNameRequest
    {
        public string ComputerName { get; init; } = string.Empty;
    }

    [HttpPost("wake-by-name")]
    public async Task<IActionResult> WakeByName([FromBody] WakeByNameRequest req, CancellationToken ct)
    {
        var accountName = User.Identity?.Name;
        if (string.IsNullOrWhiteSpace(accountName))
            return Unauthorized(new { message = "Not logged in." });

        if (string.IsNullOrWhiteSpace(req.ComputerName))
            return BadRequest(new { message = "ComputerName is required." });

        if (!_catalog.TryGetComputer(accountName, req.ComputerName, out var computer))
            return NotFound(new { message = "Computer not found." });

        try
        {
            await _wol.WakeAsync(computer, ct);
            return Ok(new { message = $"Wake sent to '{req.ComputerName}'." });
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Wake failed. Account={Account}, Computer={Computer}", accountName, req.ComputerName);
            return BadRequest(new { message = ex.Message });
        }
    }
}
