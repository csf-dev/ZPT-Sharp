using System;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.Tales
{
  /// <summary>
  /// Represents an <see cref="IModel"/> which contains TALES-specific functionality.
  /// </summary>
  public interface ITalesModel : IModel
  {
    /// <summary>
    /// Evaluate the specified TALES expression and return the result.
    /// </summary>
    /// <param name="talesExpression">The TALES expression to evaluate.</param>
    /// <param name="context">The rendering context for which we are evaluating a result.</param>
    ExpressionResult Evaluate(Expression talesExpression, RenderingContext context);

    /// <summary>
    /// Attempts to get a object instance from the root of the current model.
    /// </summary>
    /// <returns>
    /// <c>true</c>, if a root item was found matching the given <paramref name="name"/>, <c>false</c> otherwise.
    /// </returns>
    /// <param name="name">The name of the desired item.</param>
    /// <param name="context">The rendering context for which the item search is being performed.</param>
    /// <param name="result">
    /// Exposes the found object if this method returns <c>true</c>.  The value is undefined if this method returns
    /// <c>false</c>.
    /// </param>
    bool TryGetRootObject(string name, RenderingContext context, out object result);

    /// <summary>
    /// Attempts to get a object instance from the root of the current model, but only searches local variable
    /// definitions.
    /// </summary>
    /// <returns>
    /// <c>true</c>, if a root item was found matching the given <paramref name="name"/>, <c>false</c> otherwise.
    /// </returns>
    /// <param name="name">The name of the desired item.</param>
    /// <param name="element">The ZPT element.</param>
    /// <param name="result">
    /// Exposes the found object if this method returns <c>true</c>.  The value is undefined if this method returns
    /// <c>false</c>.
    /// </param>
    bool TryGetLocalRootObject(string name, ZptElement element, out object result);
  }
}

