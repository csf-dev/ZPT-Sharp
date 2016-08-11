using System;

namespace CSF.Zpt.Tales
{
  /// <summary>
  /// Delegate roughly equivalent to <c>Func&lt;T1,TResult&gt;</c>, but in which the parameter is passed by the 'ref'
  /// modifier, and not as a normal parameter.
  /// </summary>
  public delegate TResult FuncByRef<TSource,TResult>(ref TSource source) where TSource : struct;
}

