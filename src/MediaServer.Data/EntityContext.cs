using Microsoft.EntityFrameworkCore;
using MediaServer.Entities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MediaServer.Data;

public class EntityContext : DbContext
{
    public EntityContext(DbContextOptions options) : base(options) { }
    
    public DbSet<Album> Albums { get; set; }
    public DbSet<AlbumImage> AlbumImages { get; set; }
    public DbSet<Artist> Artists { get; set; }
    public DbSet<ArtistImage> ArtistImages { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Playlist> Playlists { get; set; }
    public DbSet<Track> Tracks { get; set; }

    /// <summary>
    ///     This method is called when the model for a derived context has been initialized,
    ///     but before the model has been locked down and used to initialize the context.
    ///     The default implementation of this method does nothing, but it can be overridden
    ///     in a derived class such that the model can be further configured before it is
    ///     locked down.
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
    }
}
