using System;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Gets information about a repetition/iteration.
  /// </summary>
  public class RepetitionInfo : IRepetitionInfo
  {
    #region constants

    private const string
    INDEX_IDENTIFIER          = "index",
    NUMBER_IDENTIFIER         = "number",
    EVEN_IDENTIFIER           = "even",
    ODD_IDENTIFIER            = "odd",
    START_IDENTIFIER          = "start",
    END_IDENTIFIER            = "end",
    LENGTH_IDENTIFIER         = "length",
    LETTER_IDENTIFIER         = "letter",
    UPPER_LETTER_IDENTIFIER   = "Letter";

    #endregion

    #region properties

    /// <summary>
    /// Gets a reference to the associated <see cref="IZptElement"/>, if applicable.
    /// </summary>
    /// <value>The associated element.</value>
    public IZptElement AssociatedElement
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets the name of the repetition.
    /// </summary>
    /// <value>The name.</value>
    public string Name
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets the count of items in the repetition.
    /// </summary>
    /// <value>The count.</value>
    public int Count
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets the zero-based index for the current repetition.
    /// </summary>
    /// <value>The index.</value>
    public int Index
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets the value associated with the current iteration.
    /// </summary>
    /// <value>The value.</value>
    public object Value
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets the current 'number' for the current repetition (the <see cref="Index"/> plus one).
    /// </summary>
    /// <value>The number.</value>
    public int Number
    {
      get {
        return this.Index + 1;
      }
    }

    /// <summary>
    /// True for even-indexed repetitions.
    /// </summary>
    /// <value><c>true</c> if even; otherwise, <c>false</c>.</value>
    public bool Even
    {
      get {
        return (this.Index % 2) == 0;
      }
    }

    /// <summary>
    /// True for odd-indexed repetitions.
    /// </summary>
    /// <value><c>true</c> if odd; otherwise, <c>false</c>.</value>
    public bool Odd
    {
      get {
        return (this.Index % 2) != 0;
      }
    }

    /// <summary>
    /// True for the first repetition (<see cref="Index"/> equals zero).
    /// </summary>
    /// <value><c>true</c> if this is the starting repetition; otherwise, <c>false</c>.</value>
    public bool Start
    {
      get {
        return this.Index == 0;
      }
    }

    /// <summary>
    /// True for the first repetition (<see cref="Index"/> equals <see cref="Length"/> minus one).
    /// </summary>
    /// <value><c>true</c> if this is the last repetition; otherwise, <c>false</c>.</value>
    public bool End
    {
      get {
        return (this.Index + 1) == this.Count;
      }
    }

    /// <summary>
    /// Gets the total count of all repetitions.
    /// </summary>
    /// <value><c>true</c> if length; otherwise, <c>false</c>.</value>
    public int Length
    {
      get {
        return this.Count;
      }
    }

    /// <summary>
    /// Gets an alphabetic reference for the current item.
    /// </summary>
    /// <value>The lowercase-alphabetic reference.</value>
    public string LowerLetter
    {
      get {
        return this.Index.ToAlphabeticReference();
      }
    }

    /// <summary>
    /// Gets an alphabetic reference for the current item (uppercase).
    /// </summary>
    /// <value>The uppercase-alphabetic reference.</value>
    public string UpperLetter
    {
      get {
        return this.Index.ToAlphabeticReference().ToUpperInvariant();
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Gets a specific piece of repeat information by name (a TALES path fragment).
    /// </summary>
    /// <returns><c>true</c> if the path traversal was a success; <c>false</c> otherwise.</returns>
    /// <param name="pathFragment">The path fragment.</param>
    /// <param name="result">Exposes the result if the traversal was a success</param>
    /// <param name="currentContext">Gets the current rendering context.</param>
    public bool HandleTalesPath(string pathFragment, out object result, RenderingContext currentContext)
    {
      bool output;

      switch(pathFragment)
      {
      case INDEX_IDENTIFIER:
        result = this.Index;
        output = true;
        break;

      case NUMBER_IDENTIFIER:
        result = this.Number;
        output = true;
        break;

      case EVEN_IDENTIFIER:
        result = this.Even;
        output = true;
        break;

      case ODD_IDENTIFIER:
        result = this.Odd;
        output = true;
        break;

      case START_IDENTIFIER:
        result = this.Start;
        output = true;
        break;

      case END_IDENTIFIER:
        result = this.End;
        output = true;
        break;

      case LENGTH_IDENTIFIER:
        result = this.Length;
        output = true;
        break;

      case LETTER_IDENTIFIER:
        result = this.LowerLetter;
        output = true;
        break;

      case UPPER_LETTER_IDENTIFIER:
        result = this.UpperLetter;
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

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.RepetitionInfo"/> class.
    /// </summary>
    /// <param name="name">Name.</param>
    /// <param name="index">Index.</param>
    /// <param name="count">Count.</param>
    /// <param name="value">Value.</param>
    /// <param name="element">Element.</param>
    public RepetitionInfo(string name, int index, int count, object value, IZptElement element)
    {
      if(name == null)
      {
        throw new ArgumentNullException(nameof(name));
      }

      this.Name = name;
      this.Index = index;
      this.Count = count;
      this.Value = value;
      this.AssociatedElement = element;
    }

    #endregion
  }
}

