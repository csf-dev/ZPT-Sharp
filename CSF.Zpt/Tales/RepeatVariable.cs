using System;

namespace CSF.Zpt.Tales
{
  /// <summary>
  /// Represents information about a repetition.
  /// </summary>
  public class RepeatVariable : ITalesPathHandler
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

    #region fields

    private int _index, _length;

    #endregion

    #region properties

    /// <summary>
    /// Gets the zero-based index for the current repetition.
    /// </summary>
    /// <value>The index.</value>
    public int Index
    {
      get {
        return _index;
      }
    }

    /// <summary>
    /// Gets the current 'number' for the current repetition (the <see cref="Index"/> plus one).
    /// </summary>
    /// <value>The number.</value>
    public int Number
    {
      get {
        return _index + 1;
      }
    }

    /// <summary>
    /// True for even-indexed repetitions.
    /// </summary>
    /// <value><c>true</c> if even; otherwise, <c>false</c>.</value>
    public bool Even
    {
      get {
        return (_index % 2) == 0;
      }
    }

    /// <summary>
    /// True for odd-indexed repetitions.
    /// </summary>
    /// <value><c>true</c> if odd; otherwise, <c>false</c>.</value>
    public bool Odd
    {
      get {
        return (_index % 2) != 0;
      }
    }

    /// <summary>
    /// True for the first repetition (<see cref="Index"/> equals zero).
    /// </summary>
    /// <value><c>true</c> if this is the starting repetition; otherwise, <c>false</c>.</value>
    public bool Start
    {
      get {
        return _index == 0;
      }
    }

    /// <summary>
    /// True for the first repetition (<see cref="Index"/> equals <see cref="Length"/> minus one).
    /// </summary>
    /// <value><c>true</c> if this is the last repetition; otherwise, <c>false</c>.</value>
    public bool End
    {
      get {
        return (_index + 1) == _length;
      }
    }

    /// <summary>
    /// Gets the total count of all repetitions.
    /// </summary>
    /// <value><c>true</c> if length; otherwise, <c>false</c>.</value>
    public int Length
    {
      get {
        return _length;
      }
    }

    /// <summary>
    /// Gets an alphabetic reference for the current item.
    /// </summary>
    /// <value>The lowercase-alphabetic reference.</value>
    public string LowerLetter
    {
      get {
        return _index.ToAlphabeticReference();
      }
    }

    /// <summary>
    /// Gets an alphabetic reference for the current item (uppercase).
    /// </summary>
    /// <value>The uppercase-alphabetic reference.</value>
    public string UpperLetter
    {
      get {
        return _index.ToAlphabeticReference().ToUpperInvariant();
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Gets a specific piece of repeat information by name (a TALES path fragment).
    /// </summary>
    /// <returns>The result of the path traversal.</returns>
    /// <param name="pathFragment">The path fragment.</param>
    public object HandleTalesPath(string pathFragment)
    {
      object output;

      switch(pathFragment)
      {
      case INDEX_IDENTIFIER:
        output = this.Index;
        break;

      case NUMBER_IDENTIFIER:
        output = this.Number;
        break;

      case EVEN_IDENTIFIER:
        output = this.Even;
        break;

      case ODD_IDENTIFIER:
        output = this.Odd;
        break;

      case START_IDENTIFIER:
        output = this.Start;
        break;

      case END_IDENTIFIER:
        output = this.End;
        break;

      case LENGTH_IDENTIFIER:
        output = this.Length;
        break;

      case LETTER_IDENTIFIER:
        output = this.LowerLetter;
        break;

      case UPPER_LETTER_IDENTIFIER:
        output = this.UpperLetter;
        break;

      default:
        output = null;
        break;
      }

      return output;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Tales.RepeatVariable"/> class.
    /// </summary>
    /// <param name="index">Index.</param>
    /// <param name="length">Length.</param>
    public RepeatVariable(int index, int length)
    {
      if(index < 0)
      {
        throw new ArgumentOutOfRangeException("index");
      }
      if(length < 0)
      {
        throw new ArgumentOutOfRangeException("length");
      }

      _index = index;
      _length = length;
    }

    #endregion
  }
}

