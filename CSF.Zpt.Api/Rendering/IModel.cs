using System;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// A model which represents and carries the state of a ZPT operation.
  /// </summary>
  public interface IModel
  {
    #region properties

    /// <summary>
    /// Gets information about an error, or a <c>null</c> reference if no error was encountered.
    /// </summary>
    /// <value>The error.</value>
    object Error { get; }

    /// <summary>
    /// Gets a reference to the parent model (if applicable).
    /// </summary>
    /// <value>The parent.</value>
    IModel Parent { get; }

    #endregion

    #region methods

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
    /// Adds information about a repetition to the current instance.
    /// </summary>
    /// <param name="info">The repetition information.</param>
    void AddRepetitionInfo(IRepetitionInfo info);

    /// <summary>
    /// Adds information about an encountered error to the current model instance.
    /// </summary>
    /// <param name="error">Error.</param>
    void AddError(object error);

    /// <summary>
    /// Creates and returns a child <see cref="IModel"/> instance.
    /// </summary>
    /// <returns>The child model.</returns>
    IModel CreateChildModel();

    /// <summary>
    /// Creates and returns a sibling <see cref="IModel"/> instance.
    /// </summary>
    /// <returns>The sibling model.</returns>
    IModel CreateSiblingModel();

    /// <summary>
    /// Evaluate the specified expression and return the result.
    /// </summary>
    /// <param name="expression">The expression to evaluate.</param>
    /// <param name="context">The rendering context for which we are evaluating a result.</param>
    ExpressionResult Evaluate(string expression, RenderingContext context);

    #endregion
  }
}

