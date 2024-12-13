using MediaServer.Data.Configuration;
using MediaServer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MediaServer.Data;

public class FeatureTypeConfiguration : EntityTypeConfiguration<Feature>
{
    /// <summary>
    /// Used to convert a guid to string
    /// </summary>
    public GuidToStringConverter Converter { get; }
    public FeatureTypeConfiguration(GuidToStringConverter converter)
    {
        Converter = converter;
    }
    /// <summary>
    /// Configures indexes, keys and relationships on the Feature entity
    /// </summary>
    /// <param name="builder"></param>
    public override void Configure(EntityTypeBuilder<Feature> builder)
    {
        builder?.HasKey(x => new { x.TrackId, x.ArtistId });
        builder?.Property(x => x.TrackId).HasConversion(Converter);
        builder?.Property(x => x.ArtistId).HasConversion(Converter);
        builder?.HasOne(x => x.Track)
                .WithMany(x => x.Features)
                .HasForeignKey(x => x.TrackId)
                .OnDelete(DeleteBehavior.Restrict);
        builder?.HasOne(x => x.Artist)
                .WithMany(x => x.Features)
                .HasForeignKey(x => x.ArtistId)
                .OnDelete(DeleteBehavior.Restrict);
    }
}