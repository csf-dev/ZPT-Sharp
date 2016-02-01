using System;

namespace CSF.Zpt.Tales
{
  /// <summary>
  /// Interface for a type which provides custom handling of a TALES path fragment.
  /// </summary>
  public interface ITalesPathHandler
  {
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
    /// <returns>The result of the path traversal.</returns>
    /// <param name="pathFragment">The path fragment.</param>
    object HandleTalesPath(string pathFragment);
  }
}

