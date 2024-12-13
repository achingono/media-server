namespace MediaServer.Entities;

/// <summary>
/// Represents an image used for an artist
/// </summary>
public class ArtistImage: Image
{
    /// <summary>
    /// The artist unique identifier
    /// </summary>
    public Guid ArtistId { get; set; }
    /// <summary>
    /// The artist for which this image represents
    /// </summary>
    public virtual Artist? Artist { get; set; }
}
