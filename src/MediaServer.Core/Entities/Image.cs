namespace MediaServer.Entities;

public enum Size {
    Small,
    Medium,
    Large
}

public class Image
{
    public Guid Id { get; set; }
    public Size Size { get; set; }
    public string Url { get; set; }
    public virtual ICollection<AlbumImage> AlbumImages { get; set; }
    public virtual ICollection<ArtistImage> ArtistImages { get; set; }
}
