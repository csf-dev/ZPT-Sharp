using System;
using System.IO;

namespace CSF.Zpt.Tales
{
  /// <summary>
  /// Implementation of <see cref="FilesystemDirectory"/> which contains template objects.
  /// </summary>
  public class TemplateDirectory : FilesystemDirectory
  {
    #region fields

    private ITemplateFileFactory _documentFactory;

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
    public override bool HandleTalesPath(string pathFragment, out object result)
    {
      bool output;
      object exposedResult;

      output = base.HandleTalesPath(pathFragment, out exposedResult);

      var templateFileInfo = exposedResult as FileInfo;
      if(templateFileInfo != null)
      {
        var doc = _documentFactory.CreateTemplateFile(templateFileInfo);
        exposedResult = doc;
      }

      result = exposedResult;
      return output;
    }

    /// <summary>
    /// Creates a child instance of <see cref="FilesystemDirectory"/> for the given subdirectory.
    /// </summary>
    /// <returns>The child instance.</returns>
    /// <param name="directory">Directory.</param>
    protected override FilesystemDirectory CreateChild(DirectoryInfo directory)
    {
      return new TemplateDirectory(directory, this.MandatoryExtensions);
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Tales.TemplateDirectory"/> class.
    /// </summary>
    /// <param name="directory">Directory.</param>
    /// <param name="mandatoryExtensions">If set to <c>true</c> mandatory extensions.</param>
    /// <param name="documentFactory">Document factory.</param>
    public TemplateDirectory(DirectoryInfo directory,
                             bool mandatoryExtensions = false,
                             ITemplateFileFactory documentFactory = null) : base(directory, mandatoryExtensions)
    {
      _documentFactory = documentFactory?? new ZptDocumentFactory();
    }

    #endregion
  }
}

