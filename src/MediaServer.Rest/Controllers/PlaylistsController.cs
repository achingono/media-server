using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using MediaServer.Entities;
using MediaServer.Data;

namespace MediaServer.Rest.Controllers;

[ApiController]
[Route("[controller]")]
public class PlaylistsController : ControllerBase
{
    /// <summary>
    /// Default constructor
    /// </summary>
    public PlaylistsController() { }

    /// <summary>
    /// GET /Playlists
    /// </summary>
    /// <param name="options">The query in OData format</param>
    /// <returns></returns>
    [HttpGet("")]
    [EnableQuery(MaxExpansionDepth = 3)]
    public virtual IQueryable<Playlist> GetAll([FromServices] EntityContext dbContext)
    {
        return dbContext.Playlists;
    }
}