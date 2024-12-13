using System.ComponentModel.DataAnnotations;

namespace MediaServer.Entities;

/// <summary>
/// Represents the type or category of an album
/// </summary>
public class Genre: IAuditable
{
    /// <summary>
    /// The unique identifier of the user as provided by the identity provider
    /// </summary>
    [Key]
    public Guid Id { get; set; }
    /// <summary>
    /// The name of the genre
    /// </summary>
    [Required]
    [Editable(true)]
    [DataType(DataType.Text)]
    [StringLength(50, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// The description of the genre
    /// </summary>
    [Editable(true)]
    [DataType(DataType.MultilineText)]
    [StringLength(255)]
    public string Description { get; set; } = string.Empty;
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
    public ICollection<Album>? Albums { get; set; }
}
