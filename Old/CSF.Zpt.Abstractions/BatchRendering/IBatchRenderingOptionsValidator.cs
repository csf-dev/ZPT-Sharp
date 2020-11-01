namespace CSF.Zpt.BatchRendering
{
  /// <summary>
  /// Interface for a service which renders <see cref="IBatchRenderingOptions"/> instances.
  /// </summary>
  public interface IBatchRenderingOptionsValidator
  {
    /// <summary>
    /// Validate the given options.
    /// </summary>
    /// <param name="options">The options to validate.</param>
    void Validate(IBatchRenderingOptions options);
  }
}