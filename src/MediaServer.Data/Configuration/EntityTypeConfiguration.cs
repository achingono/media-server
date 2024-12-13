namespace MediaServer.Data.Configuration;

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public abstract class EntityTypeConfiguration<TEntity> : IBuilderConfiguration, IEntityTypeConfiguration<TEntity>
    where TEntity : class
{
    /// <summary>
    /// Apply this mapping configuration
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for the database context</param>
    public virtual void ApplyConfiguration(ModelBuilder modelBuilder)
    {
        if (modelBuilder == null)
            throw new ArgumentNullException(nameof(modelBuilder));

        modelBuilder.ApplyConfiguration(this);
    }

    public abstract void Configure(EntityTypeBuilder<TEntity> builder);
}