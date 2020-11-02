using System;
using HtmlAgilityPack;

namespace CSF.Zpt.DocumentProviders
{
  /// <summary>
  /// A type which performs configuration upon the HTML Agility Pack (HAP) library, in order to fix
  /// a few quirks which make it troublesome when working with valid HTML.
  /// </summary>
  /// <remarks>
  /// <para>
  /// Beware that these configurations involve manipulating static members of the <c>HtmlAgilityPack.HtmlNode</c> type,
  /// and as such may interfere with other libraries which make use of the HAP in the same application domain.
  /// </para>
  /// </remarks>
  public class HtmlAgilityPackConfigurator
  {
    /// <summary>
    /// Perform the configuration.
    /// </summary>
    public void Configure()
    {
      // In HAP source code the form element is declared as such:
      // ElementsFlags.Add("form", HtmlElementFlag.CanOverlap | HtmlElementFlag.Empty);
      // However this means that form elements cannot have children in the DOM, which is problematic
      // See #221
      HtmlNode.ElementsFlags["form"] = HtmlElementFlag.CanOverlap;

      // In the HAP source code the option element is declared as such:
      // ElementsFlags.Add("option", HtmlElementFlag.Empty);
      // But option elements may of course have contents (it's their display value) and so the whole
      // flag can be removed.
      HtmlNode.ElementsFlags.Remove("option");
    }
  }
}
