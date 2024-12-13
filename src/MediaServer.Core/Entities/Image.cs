using System.ComponentModel.DataAnnotations;

namespace MediaServer.Entities;

public enum Size {
    Small,
    Medium,
    Large
}

public abstract class Image: IAuditable
{
    /// <summary>
    /// The unique identifier of the user as provided by the identity provider
    /// </summary>
    [Key]
    public Guid Id { get; set; }
    /// <summary>
    /// The size of the image
    /// </summary>
    public Size Size { get; set; }
    /// <summary>
    /// The url of the image
    /// </summary>
    public string Url { get; set; } = string.Empty;
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
}
