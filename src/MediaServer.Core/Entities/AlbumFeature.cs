namespace MediaServer.Entities;

public class AlbumFeature
{
    public Guid AlbumId { get; set; }
    public Guid ArtistId { get; set; }
    public virtual Album? Album { get; set; }
    public virtual Artist? Artist { get; set; }
}