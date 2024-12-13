namespace MediaServer.Data.Configuration;

using MediaServer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class ArtistImageTypeConfiguration : EntityTypeConfiguration<ArtistImage>
{
    public GuidToStringConverter Converter { get; }
    public ArtistImageTypeConfiguration(GuidToStringConverter converter)
    {
        Converter = converter;
    }
    /// <summary>
    /// Configures indexes, keys and relationships on the ArtistImage entity
    /// </summary>
    /// <param name="builder">The entity type builder</param>
    public override void Configure(EntityTypeBuilder<ArtistImage> builder)
    {
        //builder?.HasKey(x => x.Id);
        builder?.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .HasConversion(Converter);

        builder?.HasOne(x => x.Artist)
                .WithMany(x => x.ArtistImages)
                .HasForeignKey(x => x.ArtistId)
                .OnDelete(DeleteBehavior.Restrict);
                
        builder?.Property(x => x.ArtistId).HasConversion(Converter);
    }
}