    namespace MediaServer;
    
    /// <summary>
    /// The interface used for entites that have an audit trail
    /// </summary>
    public interface IAuditable : IEntity
    {
        #region Properties
        /// <summary>
        /// The date an entity was created
        /// </summary>
        DateTime CreatedOn { get; set; }

        /// <summary>
        /// The identity of the user who created the entity
        /// </summary>
        Badge CreatedBy { get; set; }

        /// <summary>
        /// The date when the entity was last updated
        /// </summary>
        DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// The identity of the user who last updated the entity
        /// </summary>
        Badge? UpdatedBy { get; set; }

        #endregion
    }