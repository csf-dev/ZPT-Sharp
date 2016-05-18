using System;
using CSF.Zpt.Metal;
using System.IO;

namespace CSF.Zpt.Tales
{
  /// <summary>
  /// Wrapper type for a <see cref="ZptDocument"/>, expressing its state which is visible to TALES.
  /// </summary>
  public class TemplateFile : ITalesPathHandler
  {
    #region constants

    private const string
      MACROS_NAME = "macros";

    #endregion

    #region fields

    private static IZptDocumentFactory _documentFactory;
    private static TemplateFileCreator _defaultCreator;
    private ZptDocument _document;

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
    ///   public object HandleTalesPath(string pathFragment)
    ///   {
    ///     switch(pathFragment)
    ///     {
    ///     case: "name";
    ///       return _person.Name;
    ///     case: "address";
    ///       return _person.Address.FullAddress;
    ///     case: "gender":
    ///       return _person.Gender.ToString();
    ///     default:
    ///       return null;
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
    public bool HandleTalesPath(string pathFragment, out object result)
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
    public TemplateFile(ZptDocument document)
    {
      if(document == null)
      {
        throw new ArgumentNullException(nameof(document));
      }

      _document = document;
    }

    /// <summary>
    /// Initializes the <see cref="CSF.Zpt.Tales.TemplateFile"/> class.
    /// </summary>
    static TemplateFile()
    {
      _documentFactory = new ZptDocumentFactory();
      _defaultCreator = DefaultCreator;
    }

    #endregion

    #region static properties

    /// <summary>
    /// Gets a default implementation of <see cref="TemplateFileCreator"/>; a delegate used to create a template
    /// file from a <c>System.IO.FileInfo</c>.
    /// </summary>
    /// <param name="sourceFile">The source file.</param>
    public static TemplateFileCreator Create { get { return _defaultCreator; } }

    #endregion

    #region static methods

    /// <summary>
    /// Default method to create template file instances.
    /// </summary>
    /// <returns>The creator.</returns>
    /// <param name="sourceFile">Source file.</param>
    private static TemplateFile DefaultCreator(FileInfo sourceFile)
    {
      if(sourceFile == null)
      {
        throw new ArgumentNullException(nameof(sourceFile));
      }
      else if(!IsSuitable(sourceFile))
      {
        string message = String.Format(Resources.ExceptionMessages.SourceFileMustBeSuitable,
                                       typeof(ZptDocument).Name);
        throw new ArgumentException(message, nameof(sourceFile));
      }

      return new TemplateFile(_documentFactory.Create(sourceFile));
    }

    /// <summary>
    /// Determines if a given source file is suitable for creating a template file or not.
    /// </summary>
    /// <returns><c>true</c> if is suitable the specified sourceFile; otherwise, <c>false</c>.</returns>
    /// <param name="sourceFile">Source file.</param>
    public static bool IsSuitable(FileInfo sourceFile)
    {
      if(sourceFile == null)
      {
        throw new ArgumentNullException(nameof(sourceFile));
      }

      return _documentFactory.SupportedFileExtensions.Contains(sourceFile.Extension);
    }

    #endregion
  }
}

