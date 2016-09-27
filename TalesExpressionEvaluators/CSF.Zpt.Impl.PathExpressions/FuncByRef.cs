using System;

namespace CSF.Zpt.Impl.PathExpressions
{
  /// <summary>
  /// Delegate roughly equivalent to <c>Func&lt;T1,TResult&gt;</c>, but in which the parameter is passed by the 'ref'
  /// modifier, and not as a normal parameter.
  /// </summary>
  internal delegate TResult FuncByRef<TSource,TResult>(ref TSource source) where TSource : struct;
}

