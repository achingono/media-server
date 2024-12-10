namespace MediaServer.Entities;

public class Playlist
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public virtual ICollection<Track> Track { get; set; }
}
