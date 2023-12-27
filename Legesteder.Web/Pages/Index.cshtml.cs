using System.Text.Json;
using System.Text.Json.Serialization;
using GeoJSON.Text.Geometry;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Legesteder.Web.Pages;

public class IndexModel : PageModel
{
    private static readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
        }
    };

    public List<Playground> Playgrounds { get; set; } = [];

    public async Task OnGet()
    {
        var projectPath = AppDomain.CurrentDomain.BaseDirectory;
        var fileFullPath = Path.Combine(projectPath, "Data", "export.geojson");
        var featureCollection = await JsonSerializer.DeserializeAsync<GeoJSON.Text.Feature.FeatureCollection>(
            System.IO.File.OpenRead(fileFullPath),
            jsonSerializerOptions);
        
        Playgrounds = featureCollection?.Features.Select(f =>
        {
            var pos = f.Geometry switch
            {
                Point p => p.Coordinates,
                //MultiPoint mp => mp.Coordinates.First(),
                Polygon p => p.Coordinates.First().Coordinates.First(),
                //MultiPolygon mp => mp.Coordinates.First().Coordinates.First(),
                _ => throw new NotSupportedException()
            };
            
            return new Playground
            {
                Longitude = pos.Longitude,
                Latitude = pos.Latitude
            };
        }).Take(10).ToList() ?? [];
    }
}

public record Playground
{
    public double Longitude { get; init; }
    public double Latitude { get; init; }
}