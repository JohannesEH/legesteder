using System.Text;
using Legesteder.Web.Models;
using Legesteder.Web.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO.VectorTiles;
using NetTopologySuite.IO.VectorTiles.Mapbox;

namespace Legesteder.Web.Pages;

public class IndexModel(IPlaygroundService playgroundService) : PageModel
{
    public List<Playground> Playgrounds { get; set; } = [];
    public string VectorTileData { get; set; } = "";

    public async Task OnGet()
    {
        Playgrounds = await playgroundService.GetPlaygroundsAsync().Take(10).ToListAsync();

        //Define which tile you want to create.
        var tileDefinition = new NetTopologySuite.IO.VectorTiles.Tiles.Tile(0, 0, 13);

        //Create a vector tile instance and pass om the tile ID from the tile definition above.
        var vt = new VectorTile { TileId = tileDefinition.Id };

        //Create one or more layers. Ideally one layer per dataset.
        var lyr = new Layer { Name = "layer1" };

        var myFeature = new Feature(new Point(12.5, 55.5), new AttributesTable
        {
            { "name", "myFeature" }
        });
        
        //Add your NTS feature(s) to the layer. Loop through all your features and add them to the tile.
        lyr.Features.Add(myFeature);

        //Add the layer to the vector tile. 
        vt.Layers.Add(lyr);

        //Output the tile to a stream. 
        using var fs = new MemoryStream();

        //Write the tile to the stream.
        vt.Write(fs);

        VectorTileData = Encoding.UTF8.GetString(fs.ToArray());
    }
}