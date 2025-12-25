using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WakeOnLan.WebApp.Services;

namespace WakeOnLan.WebApp.Pages;

[Authorize]
public sealed class IndexModel : PageModel
{
    private readonly IComputerCatalog _catalog;

    public IndexModel(IComputerCatalog catalog)
    {
        _catalog = catalog;
    }

    public string AccountName { get; private set; } = string.Empty;

    public List<ComputerRowVm> Computers { get; private set; } = new();

    public void OnGet()
    {
        AccountName = User.Identity?.Name ?? string.Empty;

        var computers = _catalog.GetComputers(AccountName);

        Computers = computers.Keys
            .OrderBy(x => x, StringComparer.OrdinalIgnoreCase)
            .Select((name, idx) => new ComputerRowVm
            {
                RowId = idx + 1,
                ComputerName = name
            })
            .ToList();
    }

    public sealed class ComputerRowVm
    {
        public int RowId { get; init; }
        public string ComputerName { get; init; } = string.Empty;
    }
}
