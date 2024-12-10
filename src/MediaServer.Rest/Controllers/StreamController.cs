using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace MediaServer.Rest.Controllers;

[ApiController]
[Route("[controller]")]
public class StreamController : ControllerBase
{
    IConfiguration Configuration { get; }
    /// <summary>
    /// Default constructor
    /// </summary>
    public StreamController(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    /// <summary>
    /// GET /Stream/{id:guid}
    /// </summary>
    /// <param name="options">The query in OData format</param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    public virtual async Task<IResult> Get(Guid id, [FromServices] EntityContext dbContext)
    {
        var track = await dbContext.Tracks.FindAsync(id);
        if (track == null) return Results.NotFound();

        var filePath = Path.Combine(Configuration.GetValue<string>("RootPath")!, track!.Path);
        if (!Path.Exists(filePath)) return Results.NotFound();

        var stream = System.IO.File.OpenRead(filePath);

        return Results.File(stream,
            contentType: GetMimeType(filePath),
            fileDownloadName: Path.GetFileName(filePath),
            enableRangeProcessing: true);
    }

    public string GetMimeType(string filePath)
    {
        const string DefaultContentType = "application/octet-stream";
        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(filePath, out string contentType))
        {
            contentType = DefaultContentType;
        }
        return contentType;
    }
}