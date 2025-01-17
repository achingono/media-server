using MediaServer.Data.Configuration;
using MediaServer.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MediaServer.Data;

public class ArtistTypeConfiguration : EntityTypeConfiguration<Artist>
{
    /// <summary>
    /// Used to convert a guid to string
    /// </summary>
    public GuidToStringConverter Converter { get; }
    public ArtistTypeConfiguration(GuidToStringConverter converter)
    {
        Converter = converter;
    }
    /// <summary>
    /// Configures indexes, keys and relationships on the Artist entity
    /// </summary>
    /// <param name="builder"></param>
    public override void Configure(EntityTypeBuilder<Artist> builder)
    {
        builder?.HasKey(x => x.Id);
        builder?.Property(x => x.Id).HasConversion(Converter);

        builder?.OwnsOne(x => x.CreatedBy).WithOwner();
        builder?.OwnsOne(x => x.UpdatedBy).WithOwner();
    }
}