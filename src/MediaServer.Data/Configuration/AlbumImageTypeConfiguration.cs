namespace MediaServer.Data.Configuration;

using MediaServer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class AlbumImageTypeConfiguration : EntityTypeConfiguration<AlbumImage>
{
    public GuidToStringConverter Converter { get; }
    public AlbumImageTypeConfiguration(GuidToStringConverter converter)
    {
        Converter = converter;
    }
    /// <summary>
    /// Configures indexes, keys and relationships on the AlbumImage entity
    /// </summary>
    /// <param name="builder">The entity type builder</param>
    public override void Configure(EntityTypeBuilder<AlbumImage> builder)
    {
        //builder?.HasKey(x => x.Id);
        builder?.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .HasConversion(Converter);

        builder?.HasOne(x => x.Album)
                .WithMany(x => x.AlbumImages)
                .HasForeignKey(x => x.AlbumId)
                .OnDelete(DeleteBehavior.Restrict);

        builder?.Property(x => x.AlbumId).HasConversion(Converter);
    }
}