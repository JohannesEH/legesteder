using System.Text.Json;
using GeoJSON.Text.Geometry;
using Legesteder.Web.Models;

namespace Legesteder.Web.Services;

public class PlaygroundService(IDawaService dawaService) : IPlaygroundService
{
    private static readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async IAsyncEnumerable<Playground> GetPlaygroundsAsync()
    {
        var projectPath = AppDomain.CurrentDomain.BaseDirectory;
        var fileFullPath = Path.Combine(projectPath, "Data", "export.geojson");
        var featureCollection = await JsonSerializer.DeserializeAsync<GeoJSON.Text.Feature.FeatureCollection>(
            File.OpenRead(fileFullPath),
            jsonSerializerOptions);

        foreach (var feature in featureCollection?.Features ?? [])
        {
            var pos = feature.Geometry switch
            {
                Point p => p.Coordinates,
                Polygon p => p.Coordinates.First().Coordinates.First(),
                //MultiPoint mp => mp.Coordinates.First(),
                //MultiPolygon mp => mp.Coordinates.First().Coordinates.First(),
                _ => throw new NotSupportedException()
            };

            var address = await dawaService.GetAddressByReverseGeocodingAsync(pos.Latitude, pos.Longitude);
            
            yield return new Playground
            {
                Longitude = pos.Longitude,
                Latitude = pos.Latitude,
                Address = address
            };
        }
    }
}