namespace CSF.Zpt.Rendering
{
    /// <summary>
    /// Helper type which permits configuration of the TAL and/or METAL models with data.
    /// </summary>
    public interface IModelValueContainer
    {
        /// <summary>
        /// Exposes access to the METAL model instance, for addition of data.
        /// </summary>
        IModelValueStore MetalModel { get; }

        /// <summary>
        /// Exposes access to the TAL model instance, for addition of data.
        /// </summary>
        IModelValueStore TalModel { get; }
    }
}