using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using MediaServer.Data;
using MediaServer.Rest.Processors;

namespace MediaServer.Rest.Controllers;

[ApiController]
[Route("[controller]")]
public class RenderController : ControllerBase
{
    IConfiguration Configuration { get; }
    /// <summary>
    /// Default constructor
    /// </summary>
    public RenderController(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    /// <summary>
    /// GET /Render
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    [HttpGet("default")]
    public virtual IResult Default(
        [FromQuery] int width,
        [FromQuery] int height)
    {
        var filePath = Configuration.GetValue<string>("DefaultCover");
        if (width > 0 || height > 0)
        {
            var imageProcessor = new ImageProcessor();
            var resizedImage = imageProcessor.ResizeImage(filePath!, width, height);
            return Results.File(resizedImage!,
                contentType: GetMimeType(resizedImage!),
                fileDownloadName: Path.GetFileName(resizedImage));
        }

        return Results.File(filePath!,
            contentType: GetMimeType(filePath!),
            fileDownloadName: Path.GetFileName(filePath));

    }

    /// <summary>
    /// GET /Render/{id:guid}
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    public virtual async Task<IResult> Get(Guid id, 
        [FromQuery] int width,
        [FromQuery] int height,
        [FromServices] EntityContext dbContext)
    {
        var image = await dbContext.Images.FindAsync(id);
        if (image == null) return Results.NotFound();

        var filePath = Path.Combine(Configuration.GetValue<string>("RootPath")!, image!.Url);
        if (!Path.Exists(filePath)) return Results.NotFound();

        // resize image if query parameter is present
        if (width > 0 || height > 0)
        {
            var imageProcessor = new ImageProcessor();
            var resizedImage = imageProcessor.ResizeImage(filePath, width, height,
                Configuration.GetValue<string>("DefaultCover"));
            return Results.File(resizedImage!,
                contentType: GetMimeType(resizedImage!),
                fileDownloadName: Path.GetFileName(resizedImage));
        }

        return Results.File(filePath,
            contentType: GetMimeType(filePath),
            fileDownloadName: Path.GetFileName(filePath));
    }

    public string GetMimeType(string filePath)
    {
        const string DefaultContentType = "application/octet-stream";
        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(filePath, out string? contentType))
        {
            contentType = DefaultContentType;
        }
        return contentType;
    }
}