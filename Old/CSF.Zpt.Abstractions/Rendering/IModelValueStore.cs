namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Represents a mechanism of configuring an <see cref="IModel"/>, by adding local and/or global variable definitions.
  /// </summary>
  public interface IModelValueStore
  {
    /// <summary>
    /// Adds a new item to the current local model, identified by a given name and containing a given value.
    /// </summary>
    /// <param name="name">The new item name.</param>
    /// <param name="value">The item value.</param>
    void AddLocal(string name, object value);

    /// <summary>
    /// Adds a new item to the current local model, identified by a given name and containing a given value.
    /// </summary>
    /// <param name="name">The new item name.</param>
    /// <param name="value">The item value.</param>
    void AddGlobal(string name, object value);

    /// <summary>
    /// Copies the state of the current model to another instance.
    /// </summary>
    /// <param name="destination">The destination model.</param>
    void CopyTo(IModelValueStore destination);
  }
}