namespace MediaServer.Entities;

public class Genre
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<Album> Albums { get; set; }
}
