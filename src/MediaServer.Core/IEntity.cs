    namespace MediaServer;

    /// <summary>
    /// The interface used for ensuring an Entity has a unique idenfier
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// The entity unique identifier
        /// </summary>
        Guid Id { get; set; }
    }