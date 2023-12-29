using Legesteder.Web.Models;

namespace Legesteder.Web.Services;

public interface IPlaygroundService
{
    IAsyncEnumerable<Playground> GetPlaygroundsAsync();
}