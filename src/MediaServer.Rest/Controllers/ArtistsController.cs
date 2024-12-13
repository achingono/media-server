using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using MediaServer.Entities;
using MediaServer.Data;

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

    /// <summary>
    /// GET /Artists/{id}
    /// </summary>
    /// <param name="options">The query in OData format</param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    public virtual async Task<IActionResult> Get(Guid id, [FromServices] EntityContext dbContext)
    {
        var artist = await dbContext.Artists.FindAsync(id);
        if (artist == null) return NotFound();
        return Ok(artist);
    }
}