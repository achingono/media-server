using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using MediaServer.Entities;
using MediaServer.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Nodes;

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

    /// <summary>
    /// GET /Playlists/{id:guid}
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public virtual async Task<IActionResult> GetOneAsync(Guid id,
    [FromServices] EntityContext dbContext)
    {
        var playlist = await dbContext.Playlists.FindAsync(id);
        if (playlist == null) return NotFound();

        return new ObjectResult(playlist);
    }

    /// <summary>
    /// POST /Playlists
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public virtual async Task<IActionResult> PostAsync([FromBody] Playlist entity,
    [FromServices] EntityContext dbContext)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        if (ModelState.IsValid)
        {
            // TODO: Add CreatedBy and CreatedOn based on current user identity
            entity.CreatedOn = DateTime.UtcNow;
            entity.CreatedBy = new Badge()
            {
                Id = Guid.NewGuid(),
                FullName = "Test User",
                Email = "test@localhost"
            };
            //After this line is executed, the Institution.Id should be populated
            var result = await dbContext.AddAsync(entity).ConfigureAwait(false);
            await dbContext.SaveChangesAsync();
            return Created(result.Entity.Id.ToString(), result.Entity);
        }

        return BadRequest(ModelState);
    }

    /// <summary>
    /// PATCH /Playlists/{id:guid}
    /// </summary>
    /// <param name="id"></param>
    /// <param name="patchDoc"></param>
    /// <param name="dbContext"></param>
    /// <returns></returns>
    [HttpPatch("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> JsonPatchWithModelState(Guid id,
    [FromBody] JsonArray operations,
    [FromServices] EntityContext dbContext)
    {
        var playlist = await dbContext.Playlists
                                    .Include(p => p.Tracks)
                                    .SingleOrDefaultAsync(p => p.Id == id);
        if (playlist == null) return NotFound();

        if (operations != null)
        {
            // manually apply changes to the entity
            operations.ToList().ForEach(op =>
            {
                if (op == null) return;

                var path = op["path"]?.GetValue<string>();
                var action = op["op"]?.GetValue<string>();
                var value = op["value"];

                if ("/tracks".Equals(path, StringComparison.OrdinalIgnoreCase))
                {
                    // value is a list of track ids
                    // get the tracks and add/remove them from the playlist
                    foreach (var node in value?.AsArray() ?? new JsonArray())
                    {
                        var trackIdString = node["id"]?.GetValue<string>();
                        if (trackIdString != null && Guid.TryParse(trackIdString, out var trackId))
                        {
                            var track = dbContext.Tracks.Find(trackId);
                            if (track != null)
                            {
                                switch (action.ToLowerInvariant())
                                {
                                    case "add":
                                        playlist.Tracks.Add(track);
                                        break;
                                    case "remove":
                                        playlist.Tracks.Remove(track);
                                        break;
                                }
                            }
                        }
                    }
                }
                else if (path == "/name" && action == "replace")
                {
                    var name = value?.GetValue<string>();
                    if (name != null)
                    {
                        playlist.Name = name;
                    }
                }
            });

            if (ModelState.IsValid)
            {
                await dbContext.SaveChangesAsync();
                return Ok();
            }
        }

        return BadRequest(ModelState);
    }

    /// <summary>
    /// GET {id}/Tracks
    /// </summary>
    /// <param name="options">The query in OData format</param>
    /// <returns></returns>
    [HttpGet("{id:guid}/tracks")]
    [EnableQuery(MaxExpansionDepth = 3)]
    public virtual IQueryable<Track> GetTracks(Guid id,
    [FromServices] EntityContext dbContext)
    {
        return dbContext.Tracks.Where(x => x.Playlists.Any(y => y.Id == id));
    }
}