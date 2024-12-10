using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using MediaServer.Entities;

namespace MediaServer.Rest.Controllers;

[ApiController]
[Route("[controller]")]
public class ImagesController : ControllerBase
{
    /// <summary>
    /// Default constructor
    /// </summary>
    public ImagesController() { }

    /// <summary>
    /// GET /Images
    /// </summary>
    /// <param name="options">The query in OData format</param>
    /// <returns></returns>
    [HttpGet("")]
    [EnableQuery(MaxExpansionDepth = 3)]
    public virtual IQueryable<Image> GetAll([FromServices] EntityContext dbContext)
    {
        return dbContext.Images;
    }
}