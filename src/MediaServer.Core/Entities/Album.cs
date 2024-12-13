using System.ComponentModel.DataAnnotations;

namespace MediaServer.Entities;

/// <summary>
/// Represents a collection of artist tracks released as an album.
/// </summary>
[DisplayColumn(nameof(Name))]
public class Album : IAuditable
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
    /// The description of the album
    /// </summary>
    [Editable(true)]
    [DataType(DataType.MultilineText)]
    [StringLength(255)]
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// The artist unique identifier
    /// </summary>
    public Guid ArtistId { get; set; }
    /// <summary>
    /// The genre unique identifier
    /// </summary>
    public Guid GenreId { get; set; }
    /// <summary>
    /// The date the album was released
    /// </summary>
    public DateTime ReleasedOn { get; set; }

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
    /// The genre of the album
    /// </summary>
    public virtual Genre? Genre { get; set; }
    /// <summary>
    /// The artist who released the album
    /// </summary>
    public virtual Artist? Artist { get; set; }
    /// <summary>
    /// The tracks released as part of the album
    /// </summary>
    public virtual ICollection<Track>? Tracks { get; set; }
    /// <summary>
    /// The images used to represent the album
    /// </summary>
    public virtual ICollection<AlbumImage>? AlbumImages { get; set; }
}
