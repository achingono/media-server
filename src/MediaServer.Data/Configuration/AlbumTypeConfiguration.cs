namespace MediaServer.Data.Configuration;

using MediaServer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class AlbumTypeConfiguration : EntityTypeConfiguration<Album>
{
        /// <summary>
        /// The converter used to convert a guid to string
        /// </summary>
        public GuidToStringConverter Converter { get; }
        public AlbumTypeConfiguration(GuidToStringConverter converter)
        {
                Converter = converter;
        }
        /// <summary>
        /// Configures indexes, keys and relationships on the Album entity
        /// </summary>
        /// <param name="builder">The entity type builder</param>
        public override void Configure(EntityTypeBuilder<Album> builder)
        {
                builder?.HasKey(x => x.Id);
                builder?.Property(x => x.Id)
                        .ValueGeneratedOnAdd()
                        .HasConversion(Converter);

                builder?.HasOne(x => x.Artist)
                        .WithMany(x => x.Albums)
                        .HasForeignKey(x => x.ArtistId)
                        .OnDelete(DeleteBehavior.Restrict);

                builder?.HasOne(x => x.Genre)
                        .WithMany(x => x.Albums)
                        .HasForeignKey(x => x.GenreId)
                        .OnDelete(DeleteBehavior.Restrict);

                builder?.HasIndex(x => new { x.Name, x.ArtistId }).IsUnique();

                builder?.OwnsOne(x => x.CreatedBy).WithOwner();
                builder?.OwnsOne(x => x.UpdatedBy).WithOwner();
        }
}