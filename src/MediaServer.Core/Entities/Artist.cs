namespace MediaServer.Entities;

public class Artist
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<Album> Albums { get; set; }
    public virtual ICollection<ArtistImage> ArtistImages { get; set; }
}
