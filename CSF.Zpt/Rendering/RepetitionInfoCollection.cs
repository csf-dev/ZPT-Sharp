using System;
using System.Collections.Generic;
using System.Linq;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Represents a collection exposing information about the <c>repeat</c> operations which are currently in effect.
  /// </summary>
  public class RepetitionInfoCollection
  {
    #region fields

    private Dictionary<NameAndElement,IRepetitionInfo> _repetitions;

    #endregion

    #region methods

    /// <summary>
    /// Makes an attempt to resolve a given value from the current instance.
    /// </summary>
    /// <returns><c>true</c>, if a value was resolved, <c>false</c> otherwise.</returns>
    /// <param name="name">The repeat variable name.</param>
    /// <param name="elementChain">A chain of <see cref="ZptElement"/>, ordered from the innermost to the othermost.</param>
    /// <param name="result">Exposes the result.</param>
    public bool TryResolveValue(string name, ZptElement[] elementChain, out object result)
    {
      bool output = false;
      result = null;

      foreach(var element in elementChain)
      {
        var nameAndElement = new NameAndElement(name, element);
        if(_repetitions.ContainsKey(nameAndElement))
        {
          result = _repetitions[nameAndElement].Value;
          output = true;
          break;
        }
      }

      return output;
    }

    /// <summary>
    /// Gets a wrapper containing the contextualised <see cref="IRepetitionInfo"/> instances for the current instance.
    /// </summary>
    /// <returns>The contextualised summaries.</returns>
    /// <param name="elementChain">Element chain.</param>
    public ContextualisedRepetitionSummaryWrapper GetContextualisedSummaries(IEnumerable<ZptElement> elementChain)
    {
      if(elementChain == null)
      {
        throw new ArgumentNullException(nameof(elementChain));
      }

      var infos = (from ele in elementChain
                   from info in _repetitions.Values
                   where
                    info.AssociatedElement == ele
                   select info);

      return new ContextualisedRepetitionSummaryWrapper(infos);
    }

    /// <summary>
    /// Creates the repetitions dictionary.
    /// </summary>
    /// <returns>The repetitions dictionary.</returns>
    /// <param name="repetitions">A collection of <see cref="RepetitionInfo"/> for the current instance.</param>
    private Dictionary<NameAndElement,IRepetitionInfo> CreateRepetitionsDictionary(IEnumerable<IRepetitionInfo> repetitions)
    {
      return this.CreateRepetitionsDictionary(repetitions, new Dictionary<NameAndElement,IRepetitionInfo>());
    }

    /// <summary>
    /// Creates the repetitions dictionary.
    /// </summary>
    /// <returns>The repetitions dictionary.</returns>
    /// <param name="repetitions">A collection of <see cref="RepetitionInfo"/> for the current instance.</param>
    /// <param name="sourceCollection">A source collection to use, as the basis for the repetition info.</param>
    private Dictionary<NameAndElement,IRepetitionInfo> CreateRepetitionsDictionary(IEnumerable<IRepetitionInfo> repetitions,
                                                                                  Dictionary<NameAndElement,IRepetitionInfo> sourceCollection)
    {
      if(sourceCollection == null)
      {
        throw new ArgumentNullException(nameof(sourceCollection));
      }
      if(repetitions == null)
      {
        throw new ArgumentNullException(nameof(repetitions));
      }

      foreach(var rep in repetitions)
      {
        sourceCollection.Add(new NameAndElement(rep.Name, rep.AssociatedElement), rep);
      }

      return sourceCollection;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.RepetitionInfoCollection"/> class.
    /// </summary>
    /// <param name="repetitions">Repetitions.</param>
    public RepetitionInfoCollection(IEnumerable<RepetitionInfo> repetitions)
    {
      if(repetitions == null)
      {
        throw new ArgumentNullException(nameof(repetitions));
      }

      _repetitions = this.CreateRepetitionsDictionary(repetitions);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.RepetitionInfoCollection"/> class.
    /// </summary>
    /// <param name="parentCollection">Parent collection.</param>
    /// <param name="repetitions">Repetitions.</param>
    public RepetitionInfoCollection(RepetitionInfoCollection parentCollection,
                                    IEnumerable<IRepetitionInfo> repetitions = null)
    {
      if(parentCollection == null)
      {
        throw new ArgumentNullException(nameof(parentCollection));
      }

      var source = new Dictionary<NameAndElement,IRepetitionInfo>(parentCollection._repetitions);
      var newElements = repetitions?? new RepetitionInfo[0];

      _repetitions = this.CreateRepetitionsDictionary(newElements, source);
    }

    #endregion

    #region contained type

    class NameAndElement : IEquatable<NameAndElement>
    {
      private Tuple<string,ZptElement> _tuple;

      string Name { get { return _tuple.Item1; } }

      ZptElement Element { get { return _tuple.Item2; } }

      public bool Equals(NameAndElement other)
      {
        return (!Object.ReferenceEquals(null, other)
                && this.Name.Equals(other.Name)
                && this.Element.Equals(other.Element));
      }

      public override bool Equals(object obj)
      {
        return this.Equals(obj as NameAndElement);
      }

      public override int GetHashCode()
      {
        return this.Name.GetHashCode() ^ this.Element.GetHashCode();
      }

      internal NameAndElement(string name, ZptElement element)
      {
        if(name == null)
        {
          throw new ArgumentNullException(nameof(name));
        }
        if(element == null)
        {
          throw new ArgumentNullException(nameof(element));
        }

        _tuple = new Tuple<string, ZptElement>(name, element);
      }
    }

    #endregion
  }
}

