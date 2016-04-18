using System;
using System.Collections.Generic;
using System.Collections;

namespace CSF.Zpt.Tales
{
  /// <summary>
  /// Implementation of <see cref="NamedObjectWrapper"/> which also implements <c>IEnumerable&lt;object&gt;</c>.
  /// </summary>
  public class EnumerableObjectWrapperWithNamedItems : NamedObjectWrapper, IEnumerable<object>
  {
    #region fields

    private ICollection<object> _items;

    #endregion

    #region properties

    /// <summary>
    /// Exposes the items in this enumerable collection.
    /// </summary>
    /// <value>The items.</value>
    public ICollection<object> Items
    {
      get {
        return _items;
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Gets the enumerator for the current collection.
    /// </summary>
    /// <returns>The enumerator.</returns>
    public virtual IEnumerator<object> GetEnumerator()
    {
      return _items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Tales.EnumerableObjectWrapperWithNamedItems"/> class.
    /// </summary>
    public EnumerableObjectWrapperWithNamedItems()
    {
      _items = new HashSet<object>();
    }

    #endregion
  }
}

