using Legesteder.Web.Models;
using Legesteder.Web.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Legesteder.Web.Pages;

public class IndexModel(IPlaygroundService playgroundService) : PageModel
{
    public List<Playground> Playgrounds { get; set; } = [];

    public async Task OnGet()
    {
        Playgrounds = await playgroundService.GetPlaygroundsAsync().Take(10).ToListAsync();
    }
}