namespace MediaServer.Entities;

/// <summary>
/// Represents an image used for an artist's album
/// </summary>
public class AlbumImage : Image
{
    /// <summary>
    /// The album unique identifier
    /// </summary>
    public Guid AlbumId { get; set; }
    /// <summary>
    /// The album for which this image represents
    /// </summary>
    public virtual Album? Album { get; set; }
}
