namespace Legesteder.Web.Models;

public record Playground
{
    public double Longitude { get; init; }
    public double Latitude { get; init; }
    public string Address { get; init; }
}