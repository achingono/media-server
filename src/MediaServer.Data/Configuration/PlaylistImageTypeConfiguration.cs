namespace MediaServer.Data.Configuration;

using MediaServer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class PlaylistImageTypeConfiguration : EntityTypeConfiguration<PlaylistImage>
{
    public GuidToStringConverter Converter { get; }
    public PlaylistImageTypeConfiguration(GuidToStringConverter converter)
    {
        Converter = converter;
    }
    /// <summary>
    /// Configures indexes, keys and relationships on the PlaylistImage entity
    /// </summary>
    /// <param name="builder">The entity type builder</param>
    public override void Configure(EntityTypeBuilder<PlaylistImage> builder)
    {
        //builder?.HasKey(x => x.Id);
        builder?.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .HasConversion(Converter);

        builder?.HasOne(x => x.Playlist)
                .WithMany(x => x.PlaylistImages)
                .HasForeignKey(x => x.PlaylistId)
                .OnDelete(DeleteBehavior.Restrict);

        builder?.Property(x => x.PlaylistId).HasConversion(Converter);

        builder?.OwnsOne(x => x.CreatedBy).WithOwner();
        builder?.OwnsOne(x => x.UpdatedBy).WithOwner();
    }
}