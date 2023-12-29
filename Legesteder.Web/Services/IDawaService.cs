namespace Legesteder.Web.Services;

public interface IDawaService
{
    Task<string> GetAddressByReverseGeocodingAsync(double latitude, double longitude);
}