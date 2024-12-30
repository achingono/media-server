namespace MediaServer.Entities;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// Represents a song in an album or playlist
/// </summary>
public class Track
{
    /// <summary>
    /// The unique identifier of the user as provided by the identity provider
    /// </summary>
    [Key]
    public Guid Id { get; set; }
    /// <summary>
    /// The name of the album
    /// </summary>
    [Required]
    [Editable(true)]
    [DataType(DataType.Text)]
    [StringLength(50, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// The position of the track in the album
    /// </summary>
    public int Number { get; set; }
    /// <summary>
    /// The length of the track in seconds
    /// </summary>
    public int Duration { get; set; }
    /// <summary>
    /// The location of the track in the file system
    /// </summary>
    public string Path { get; set; } = string.Empty;
    /// <summary>
    /// The date this object was created
    /// </summary>
    [ScaffoldColumn(false)]
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// The details of the user who created this object
    /// </summary>
    [ScaffoldColumn(false)]
    public Badge CreatedBy { get; set; } = new();

    /// <summary>
    /// The date this object was last updated
    /// </summary>
    [ScaffoldColumn(false)]
    public DateTime? UpdatedOn { get; set; }

    /// <summary>
    /// The details of the user who last updated this object
    /// </summary>
    [ScaffoldColumn(false)]
    public Badge? UpdatedBy { get; set; } = null!;
    /// <summary>
    /// The unique identifier of the album
    /// </summary>
    public Guid AlbumId { get; set; }
    /// <summary>
    /// The album this track belongs to
    /// </summary>
    public virtual Album? Album { get; set; }
    /// <summary>
    /// The artists who performed this track
    /// </summary>
    public virtual ICollection<TrackFeature>? Features { get; set; }
    /// <summary>
    /// The playlists in which this track is added
    /// </summary>
    public virtual ICollection<Playlist>? Playlists { get; set; }
}
