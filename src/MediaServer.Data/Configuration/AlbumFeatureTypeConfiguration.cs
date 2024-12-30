using MediaServer.Data.Configuration;
using MediaServer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MediaServer.Data;

public class AlbumFeatureTypeConfiguration : EntityTypeConfiguration<AlbumFeature>
{
    /// <summary>
    /// Used to convert a guid to string
    /// </summary>
    public GuidToStringConverter Converter { get; }
    public AlbumFeatureTypeConfiguration(GuidToStringConverter converter)
    {
        Converter = converter;
    }
    /// <summary>
    /// Configures indexes, keys and relationships on the Feature entity
    /// </summary>
    /// <param name="builder"></param>
    public override void Configure(EntityTypeBuilder<AlbumFeature> builder)
    {
        builder?.HasKey(x => new { x.AlbumId, x.ArtistId });
        builder?.Property(x => x.AlbumId).HasConversion(Converter);
        builder?.Property(x => x.ArtistId).HasConversion(Converter);
        builder?.HasOne(x => x.Album)
                .WithMany(x => x.AlbumFeatures)
                .HasForeignKey(x => x.AlbumId)
                .OnDelete(DeleteBehavior.Restrict);
        builder?.HasOne(x => x.Artist)
                .WithMany(x => x.AlbumFeatures)
                .HasForeignKey(x => x.ArtistId)
                .OnDelete(DeleteBehavior.Restrict);
    }
}