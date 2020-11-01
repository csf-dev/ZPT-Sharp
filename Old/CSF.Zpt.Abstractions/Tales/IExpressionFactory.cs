using System;

namespace CSF.Zpt.Tales
{
  /// <summary>
  /// Creator for <see cref="Expression"/> instances.
  /// </summary>
  public interface IExpressionFactory
  {
    /// <summary>
    /// Create an expression from the specified prefix and content.
    /// </summary>
    /// <param name="prefix">Prefix.</param>
    /// <param name="content">Content.</param>
    Expression Create(string prefix, string content);

    /// <summary>
    /// Create an expression from the specified source string.
    /// </summary>
    /// <param name="source">Source.</param>
    Expression Create(string source);

    /// <summary>
    /// Create an expression from the content of another expression.
    /// </summary>
    /// <param name="expression">The expression from which to create another expression.</param>
    Expression Create(Expression expression);
  }
}

