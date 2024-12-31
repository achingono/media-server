namespace MediaServer.Entities;

/// <summary>
/// Represents an image used for an artist's playlist
/// </summary>
public class PlaylistImage : Image
{
    /// <summary>
    /// The playlist unique identifier
    /// </summary>
    public Guid PlaylistId { get; set; }
    /// <summary>
    /// The playlist for which this image represents
    /// </summary>
    public virtual Playlist? Playlist { get; set; }
}
