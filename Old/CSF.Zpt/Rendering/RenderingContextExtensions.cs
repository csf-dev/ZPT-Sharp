using System;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Extension methods for <see cref="RenderingContext"/> instances.
  /// </summary>
  public static class RenderingContextExtensions
  {
    /// <summary>
    /// Gets a METAL attribute which matches the given criteria, or a <c>null</c> reference is no matching attribute is
    /// found.
    /// </summary>
    /// <returns>The attribute, or a <c>null</c> reference.</returns>
    /// <param name="context">The rendering context from which to get attributes.</param>
    /// <param name="attributeName">The attribute name.</param>
    public static IZptAttribute GetMetalAttribute(this IRenderingContext context, string attributeName)
    {
      if(context == null)
      {
        throw new ArgumentNullException(nameof(context));
      }

      return context.GetAttribute(ZptConstants.Metal.Namespace, attributeName);
    }

    /// <summary>
    /// Gets a TAL attribute which matches the given criteria, or a <c>null</c> reference is no matching attribute is
    /// found.
    /// </summary>
    /// <returns>The attribute, or a <c>null</c> reference.</returns>
    /// <param name="context">The rendering context from which to get attributes.</param>
    /// <param name="attributeName">The attribute name.</param>
    public static IZptAttribute GetTalAttribute(this IRenderingContext context, string attributeName)
    {
      if(context == null)
      {
        throw new ArgumentNullException(nameof(context));
      }

      return context.GetAttribute(ZptConstants.Tal.Namespace, attributeName);
    }
  }
}

