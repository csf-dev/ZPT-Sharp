﻿using System;
using CSF.Zpt.Rendering;
using CSF.Zpt.Tales;

namespace CSF.Zpt.ExpressionEvaluators
{
  /// <summary>
  /// Specialisation of <see cref="PathExpressionEvaluator"/> which only looks in local variable definitions.
  /// </summary>
  public class LocalVariablePathExpressionEvaluator : PathExpressionEvaluator
  {
    #region constants

    private static readonly string Prefix = "local";

    #endregion

    #region properties

    /// <summary>
    /// Gets the expression prefix handled by the current evaluator instance.
    /// </summary>
    /// <value>The prefix.</value>
    public override string ExpressionPrefix
    {
      get {
        return Prefix;
      }
    }

    #endregion

    #region overrides

    /// <summary>
    /// Attempts to get a root object, from which to begin traversal of the path.
    /// </summary>
    /// <returns><c>true</c>, if the root object was retrieved, <c>false</c> otherwise.</returns>
    /// <param name="walker">A TALES path walker.</param>
    /// <param name="element">A ZPT element.</param>
    /// <param name="model">The TALES model.</param>
    /// <param name="result">Exposes the result of this operation.</param>
    protected virtual bool TryGetTraversalRoot(PathWalker walker,
                                               ZptElement element,
                                               TalesModel model,
                                               out object result)
    {
      bool output;

      if(walker.NextPart()
         && model.TryGetLocalRootObject(walker.CurrentPart.Value, element, out result))
      {
        output = true;
      }
      else
      {
        output = false;
        result = null;
      }

      return output;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.ExpressionEvaluators.LocalVariablePathExpressionEvaluator"/> class.
    /// </summary>
    public LocalVariablePathExpressionEvaluator() : base() {}

    #endregion
  }
}
