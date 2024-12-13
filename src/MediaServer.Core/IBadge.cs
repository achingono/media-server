namespace MediaServer;

/// <summary>
/// The interface used for storing a user's identity information
/// </summary>
public interface IBadge
{
    /// <summary>
    /// The unique identifier of the user as provided by the identity provider
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// The user's email addres
    /// </summary>
    string Email { get; }

    /// <summary>
    /// The user's full name
    /// </summary>
    string FullName { get; }
}