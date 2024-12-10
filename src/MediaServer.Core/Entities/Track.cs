namespace MediaServer.Entities;

public class Track
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Number { get; set; }
    public int Duration { get; set; }
    public string Path { get; set; }
    public Album Album { get; set; }
    public Artist Artist { get; set; }
    public virtual ICollection<Playlist> Playlists { get; set; }
}
