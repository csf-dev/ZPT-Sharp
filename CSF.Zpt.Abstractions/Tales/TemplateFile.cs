using System;
using System.IO;

namespace CSF.Zpt.Tales
{
  /// <summary>
  /// Wrapper type for a <see cref="IZptDocument"/>, expressing its state which is visible to TALES.
  /// </summary>
  public class TemplateFile : ITalesPathHandler
  {
    #region constants

    private const string
      MACROS_NAME = "macros";

    #endregion

    #region fields

    private IZptDocument _document;

    #endregion

    #region methods

    /// <summary>
    /// Gets an <c>System.Object</c> based upon a TALES path fragment.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method should return a <c>System.Object</c> which represents the traversal of a single-level TALES path
    /// fragment, from the current instance.  The value of that fragment is passed via the name
    /// <paramref name="pathFragment"/>.
    /// </para>
    /// <para>
    /// The precise meaning of 'traversal' is left to the implementation, but typical semantics will see an object
    /// return an associated object from an object graph.
    /// </para>
    /// <example>
    /// <para>
    /// In this simple example, the <c>Employee</c> class may return data from a related <c>Person</c> object, without
    /// exposing the Person object directly.  This might be because (as shown in this example), the API of that
    /// <c>Person</c> object is more complex than desired, and so TALES should see a simplified version.
    /// </para>
    /// <code>
    /// public class Employee : ITalesPathHandler
    /// {
    ///   private Person _person;
    ///   
    ///   public bool HandleTalesPath(string pathFragment, out object result, RenderingContext currentContext)
    ///   {
    ///     switch(pathFragment)
    ///     {
    ///     case: "name";
    ///       result = _person.Name;
    ///       return true;
    ///     case: "address";
    ///       result = _person.Address.FullAddress;
    ///       return true;
    ///     case: "gender":
    ///       result = _person.Gender.ToString();
    ///       return true;
    ///     default:
    ///       result = null;
    ///       return false;
    ///     }
    ///   }
    /// }
    /// </code>
    /// </example>
    /// <para>
    /// Note that the return value does not need to be a primitive type.  It may be a complex object, and the return
    /// value may also implement <see cref="ITalesPathHandler"/> if desired.
    /// </para>
    /// </remarks>
    /// <returns><c>true</c> if the path traversal was a success; <c>false</c> otherwise.</returns>
    /// <param name="pathFragment">The path fragment.</param>
    /// <param name="result">Exposes the result if the traversal was a success</param>
    /// <param name="currentContext">Gets the current rendering context.</param>
    public bool HandleTalesPath(string pathFragment, out object result, Rendering.IRenderingContext currentContext)
    {
      bool output;

      switch(pathFragment)
      {
      case MACROS_NAME:
        result = _document.GetMacros();
        output = true;
        break;

      default:
        result = null;
        output = false;
        break;
      }

      return output;
    }

    #endregion

    #region constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Tales.TemplateFile"/> class.
    /// </summary>
    /// <param name="document">Document.</param>
    public TemplateFile(IZptDocument document)
    {
      if(document == null)
      {
        throw new ArgumentNullException(nameof(document));
      }

      _document = document;
    }

    #endregion
  }
}

