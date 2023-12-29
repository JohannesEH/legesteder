using System.Globalization;
using System.Text.Json;

namespace Legesteder.Web.Services;

public class DawaService(HttpClient httpClient) : IDawaService
{
    private static readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<string> GetAddressByReverseGeocodingAsync(double latitude, double longitude)
    {
        var url = string.Create(CultureInfo.InvariantCulture, $"adgangsadresser/reverse?x={longitude}&y={latitude}&struktur=mini");
        var result = await httpClient.GetFromJsonAsync<DawaReverseGeocodingResult>(url, jsonSerializerOptions);
        return result?.Betegnelse ?? "";
    }

    private record DawaReverseGeocodingResult(string Betegnelse);
}