namespace MediaServer.Entities;

public class TrackFeature
{
    public Guid TrackId { get; set; }
    public Guid ArtistId { get; set; }
    public virtual Track? Track { get; set; }
    public virtual Artist? Artist { get; set; }
}