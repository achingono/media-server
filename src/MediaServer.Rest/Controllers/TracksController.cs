using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using MediaServer.Entities;

namespace MediaServer.Rest.Controllers;

[ApiController]
[Route("[controller]")]
public class TracksController : ControllerBase
{
    /// <summary>
    /// Default constructor
    /// </summary>
    public TracksController() { }

    /// <summary>
    /// GET /Tracks
    /// </summary>
    /// <param name="options">The query in OData format</param>
    /// <returns></returns>
    [HttpGet("")]
    [EnableQuery(MaxExpansionDepth = 3)]
    public virtual IQueryable<Track> GetAll([FromServices] EntityContext dbContext)
    {
        return dbContext.Tracks;
    }
}