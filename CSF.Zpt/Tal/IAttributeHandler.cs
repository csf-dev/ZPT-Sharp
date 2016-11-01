using System;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.Tal
{
  /// <summary>
  /// Interface for a type which handles a single TAL attribute present upon a <see cref="ZptElement"/>, exposed by
  /// its <see cref="IRenderingContext"/>.
  /// </summary>
  public interface IAttributeHandler
  {
    /// <summary>
    /// Handle the related attribute types which exist upon the element exposed by the given context, if any.
    /// </summary>
    /// <returns>A response type providing information about the result of this operation.</returns>
    /// <param name="context">The rendering context, which exposes a ZPT element.</param>
    AttributeHandlingResult Handle(IRenderingContext context);
  }
}

