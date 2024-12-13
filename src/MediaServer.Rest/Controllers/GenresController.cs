using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using MediaServer.Entities;
using MediaServer.Data;

namespace MediaServer.Rest.Controllers;

[ApiController]
[Route("[controller]")]
public class GenresController : ControllerBase
{
    /// <summary>
    /// Default constructor
    /// </summary>
    public GenresController() { }

    /// <summary>
    /// GET /Genres
    /// </summary>
    /// <param name="options">The query in OData format</param>
    /// <returns></returns>
    [HttpGet("")]
    [EnableQuery(MaxExpansionDepth = 3)]
    public virtual IQueryable<Genre> GetAll([FromServices] EntityContext dbContext)
    {
        return dbContext.Genres;
    }
}