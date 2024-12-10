using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using MediaServer.Entities;

namespace MediaServer.Rest.Controllers;

[ApiController]
[Route("[controller]")]
public class ArtistsController : ControllerBase
{
    /// <summary>
    /// Default constructor
    /// </summary>
    public ArtistsController() { }

    /// <summary>
    /// GET /Artists
    /// </summary>
    /// <param name="options">The query in OData format</param>
    /// <returns></returns>
    [HttpGet("")]
    [EnableQuery(MaxExpansionDepth = 3)]
    public virtual IQueryable<Artist> GetAll([FromServices] EntityContext dbContext)
    {
        return dbContext.Artists;
    }
}