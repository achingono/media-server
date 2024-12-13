namespace MediaServer.Data.Configuration;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// Represents database context model mapping configuration
/// </summary>
public interface IBuilderConfiguration
{
    /// <summary>
    /// Apply this mapping configuration
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for the database context</param>
    void ApplyConfiguration(ModelBuilder modelBuilder);
}