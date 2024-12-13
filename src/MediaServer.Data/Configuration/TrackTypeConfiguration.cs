using MediaServer.Data.Configuration;
using MediaServer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MediaServer.Data;

public class TrackTypeConfiguration : EntityTypeConfiguration<Track>
{
    /// <summary>
    /// Used to convert a guid to string
    /// </summary>
    public GuidToStringConverter Converter { get; }
    public TrackTypeConfiguration(GuidToStringConverter converter)
    {
        Converter = converter;
    }
    /// <summary>
    /// Configures indexes, keys and relationships on the Track entity
    /// </summary>
    /// <param name="builder"></param>
    public override void Configure(EntityTypeBuilder<Track> builder)
    {
        builder?.HasKey(x => x.Id);
        builder?.Property(x => x.Id).HasConversion(Converter);
        builder?.HasOne(x => x.Album)
                .WithMany(x => x.Tracks)
                .OnDelete(DeleteBehavior.Restrict);
    }
}