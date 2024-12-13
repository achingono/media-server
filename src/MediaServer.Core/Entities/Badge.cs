namespace MediaServer;

/// <summary>
/// Represents a user who performed an action in the system
/// </summary>
public class Badge : IBadge
{
    /// <summary>
    /// The unique identifier of the user as provided by the identity provider
    /// </summary>
    public Guid Id { get; set; } = default!;
    /// <summary>
    /// The email address of the user
    /// </summary>
    public string Email { get; set; } = default!;
    /// <summary>
    /// The user's full name
    /// </summary>
    public string FullName { get; set; } = default!;
}