namespace MediaServer.Entities;

public class Album
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Artist Artist { get; set; }
    public DateTime DateReleased { get; set; }    
    public Genre Genre { get; set; }
    public virtual ICollection<Track> Tracks { get; set; }
    public virtual ICollection<AlbumImage> AlbumImages { get; set; }
}
