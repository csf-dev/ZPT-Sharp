//  
//  RepeatVariable.cs
//  
//  Author:
//       Craig Fowler <craig@craigfowler.me.uk>
// 
//  Copyright (c) 2010 Craig Fowler
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections;
using CraigFowler.Web.ZPT.Tales;
using System.Text;

namespace CraigFowler.Web.ZPT.Tal
{
  /// <summary>
  /// <para>Represents a repeat variable for use with a TAL 'repeat' directive.</para>
  /// </summary>
  public class RepeatVariable : IEnumerator
  {
    #region constants
    
    private const string
      ALPHABET                = "abcdefghijklmnopqrstuvwxyz";
    
    #endregion
    
    #region fields
    
    private IList underlyingCollection;
    private int index;
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// <para>Read-only.  Gets the index of the current iteration (zero-based).</para>
    /// </summary>
    [TalesMember("index")]
    public int Index
    {
      get {
        return index;
      }
      private set {
        if(value < -1)
        {
          throw new ArgumentOutOfRangeException("value", "Index cannot be less than -1");
        }
        
        index = value;
      }
    }
    
    /// <summary>
    /// <para>
    /// Read-only.  Gets the index of the current iteration based at 1.  This will always equal <see cref="Index"/>
    /// plus one.
    /// </para>
    /// </summary>
    [TalesMember("number")]
    public int Number
    {
      get {
        return this.Index + 1;
      }
    }
    
    /// <summary>
    /// <para>Read-only.  Gets whether the <see cref="Index"/> indicates an even-numbered iteration.</para>
    /// </summary>
    [TalesMember("even")]
    public bool Even
    {
      get {
        CheckThatIndexIsInRange();
        return Math.Round((float) this.Index / 2f) == ((float) this.Index / 2f);
      }
    }
    
    /// <summary>
    /// <para>Read-only.  Gets whether the <see cref="Index"/> indicates an odd-numbered iteration.</para>
    /// </summary>
    [TalesMember("odd")]
    public bool Odd
    {
      get {
        return !this.Even;
      }
    }
    
    /// <summary>
    /// <para>
    /// Read-only.  Gets whether the <see cref="Index"/> that we are at the start of the
    /// <see cref="UnderlyingCollection"/>.
    /// </para>
    /// </summary>
    [TalesMember("start")]
    public bool Start
    {
      get {
        CheckThatIndexIsInRange();
        return this.Index == 0;
      }
    }
    
    /// <summary>
    /// <para>
    /// Read-only.  Gets whether the <see cref="Index"/> that we are at the end of the
    /// <see cref="UnderlyingCollection"/>.
    /// </para>
    /// </summary>
    [TalesMember("end")]
    public bool End
    {
      get {
        CheckThatIndexIsInRange();
        return this.Index == (this.Length - 1);
      }
    }
    
    /// <summary>
    /// <para>Read-only.  Gets the length of the <see cref="UnderlyingCollection"/>.</para>
    /// </summary>
    [TalesMember("length")]
    public int Length
    {
      get {
        return this.UnderlyingCollection.Count;
      }
    }
    
    /// <summary>
    /// <para>
    /// Read-only.  Gets a <see cref="System.String"/> representation of the <see cref="Index"/> in lowercase.
    /// </para>
    /// <seealso cref="GenerateLetterReference(int)"/>
    /// </summary>
    [TalesMember("letter")]
    public string Letter
    {
      get {
        CheckThatIndexIsInRange();
        return GenerateLetterReference(this.Index);
      }
    }
    
    /// <summary>
    /// <para>
    /// Read-only.  Gets a <see cref="System.String"/> representation of the <see cref="Index"/> in uppercase.
    /// </para>
    /// <seealso cref="Letter"/>
    /// <seealso cref="GenerateLetterReference(int)"/>
    /// </summary>
    /// <remarks>
    /// <para>This property returns an uppercase version of <see cref="Letter"/>.</para>
    /// </remarks>
    [TalesMember("Letter")]
    public string UppercaseLetter
    {
      get {
        return this.Letter.ToUpper();
      }
    }
    
    /// <summary>
    /// <para>
    /// Read-only.  Gets the instance within <see cref="UnderlyingCollection"/> indicated by the current
    /// <see cref="Index"/>.
    /// </para>
    /// </summary>
    [TalesMember(true)]
    public object Current
    {
      get {
        CheckThatIndexIsInRange();
        return this.UnderlyingCollection[this.Index];
      }
    }
    
    /// <summary>
    /// <para>
    /// Read-only.  Gets the <see cref="IList"/> that provides the underlying collection for this instance of
    /// <see cref="RepeatVariable"/>.
    /// </para>
    /// </summary>
    [TalesMember(true)]
    protected IList UnderlyingCollection
    {
      get {
        return underlyingCollection;
      }
      private set {
        if(value == null)
        {
          throw new ArgumentNullException("value");
        }
        
        underlyingCollection = value;
      }
    }
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// <para>Moves the <see cref="Index"/> to the next item in the collection.</para>
    /// </summary>
    /// <returns>
    /// A <see cref="System.Boolean"/>, indicating whether there was another item within the
    /// <see cref="UnderlyingCollection"/>.  If this action would have placed <see cref="Index"/> past the end of the
    /// underlying collection then this method returns false.
    /// </returns>
    [TalesMember(true)]
    public bool MoveNext()
    {
      this.Index ++;
      return this.Index < this.Length;
    }
    
    /// <summary>
    /// <para>This method should never be used and will always throw a <see cref="NotSupportedException"/>.</para>
    /// </summary>
    /// <remarks>
    /// <para>This method signature is only included as it is required by <see cref="IEnumerator"/>.</para>
    /// </remarks>
    /// <exception cref="NotSupportedException">
    /// This method will ALWAYS throw an exception.  It should not be used.
    /// </exception>
    [TalesMember(true)]
    public void Reset()
    {
      throw new NotSupportedException("Resetting the repeat variable is not supported");
    }
    
    /// <summary>
    /// <para>
    /// Performs a sanity-check that the <see cref="Index"/> is in range.  That is, it is not less than zero and that it
    /// is less than the length of the <see cref="UnderlyingCollection"/>.
    /// </para>
    /// </summary>
    [TalesMember(true)]
    private void CheckThatIndexIsInRange()
    {
      if(this.Index < 0)
      {
        throw new InvalidOperationException("The index is currently positioned before the start of the collection. " +
                                            "Have you forgotten to use 'MoveNext()'?");
      }
      else if(this.Index >= this.Length)
      {
        throw new InvalidOperationException("The index is currently positioned beyond the end of the collection.");
      }
    }
    
    #endregion
    
    #region constructor
    
    /// <summary>
    /// <para>Instantiates this instance with a given <paramref name="list"/>.</para>
    /// </summary>
    /// <param name="list">
    /// An <see cref="IList"/> that will be used as the <see cref="UnderlyingCollection"/> for this instance.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// If the <paramref name="list"/> is null.
    /// </exception>
    public RepeatVariable(IList list)
    {
      this.UnderlyingCollection = list;
      this.Index = -1;
    }
    
    /// <summary>
    /// <para>Instantiates this instance with a given <paramref name="list"/>.</para>
    /// </summary>
    /// <param name="list">
    /// A <see cref="System.Object"/> that shall be used as the <see cref="UnderlyingCollection"/> for this instance.
    /// This parameter must be convertable into <see cref="IList"/>.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// If the <paramref name="list"/> is null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If the <paramref name="list"/> could not be converted to <see cref="IList"/>.
    /// </exception>
    public RepeatVariable(object list)
    {
      if(list == null)
      {
        throw new ArgumentNullException("list");
      }
      else if(!(list is IList))
      {
        throw new ArgumentOutOfRangeException("list", "List could not be converted to System.Collections.IList.");
      }
      
      this.UnderlyingCollection = list as IList;
      this.Index = -1;
    }
    
    #endregion
    
    #region static methods
    
    /// <summary>
    /// <para>Overloaded.  Generates a letter reference for a given <paramref name="numericIndex"/>.</para>
    /// </summary>
    /// <example>
    /// <para>If <paramref name="numericIndex"/> is <c>500</c> then this method would return <c>tg</c>.</para>
    /// </example>
    /// <param name="numericIndex">
    /// A zero-based <see cref="System.Int32"/>, the numeric index for which this string reference will be generated.
    /// </param>
    /// <returns>
    /// A <see cref="System.String"/> derived from the <paramref name="numericIndex"/> using the scheme <c>a</c> to
    /// <c>z</c> and then <c>aa</c> to <c>az</c>, <c>ba</c> to <c>bz</c> and so on.
    /// </returns>
    public static string GenerateLetterReference(int numericIndex)
    {
      StringBuilder output = new StringBuilder();
      
      GenerateLetterReference(numericIndex, 0, ref output);
      
      return output.ToString();
    }
    
    /// <summary>
    /// <para>
    /// Overloaded.  Generates a string equivalent to a given <paramref name="numericIndex"/>.  This method is recursive
    /// where appropriate.
    /// </para>
    /// </summary>
    /// <param name="numericIndex">
    /// A zero-based <see cref="System.Int32"/>, the numeric index for which this string reference will be generated.
    /// </param>
    /// <param name="characterPosition">
    /// <para>
    /// A <see cref="System.Int32"/> denoting the position of the character to be generated by this iteration of this
    /// recursive method.
    /// </para>
    /// </param>
    /// <param name="output">
    /// A <see cref="StringBuilder"/>, used to provide the output.  This must be a valid, non-null reference.
    /// </param>
    /// <remarks>
    /// <para>
    /// The <paramref name="characterPosition"/>, representing the current position is zero-based.
    /// </para>
    /// <example>
    /// <code>
    /// abc
    ///   ^
    /// Character position 0
    /// </code>
    /// </example>
    /// <example>
    /// <code>
    /// abc
    /// ^
    /// Character position 2
    /// </code>
    /// </example>
    /// </remarks>
    private static void GenerateLetterReference(int numericIndex, int characterPosition, ref StringBuilder output)
    {
      double character, truncatedIndex;
      int alphabetLength = ALPHABET.Length;
      
      // Sanity-check our input
      if(numericIndex < 0)
      {
        throw new ArgumentOutOfRangeException("numericIndex", "Index cannot be less than zero");
      }
      else if(characterPosition < 0)
      {
        throw new ArgumentOutOfRangeException("characterPosition", "Character position cannot be less than zero");
      }
      else if(output == null)
      {
        throw new ArgumentNullException("output");
      }
      
      /* Calculate the truncated index for this value of characterPosition.
       * 
       * We divide numericIndex by ([the length of the alphabet] to the power of [the characterPosition]) and then
       * discard any digits after the decimal point.  This both normalises the numericIndex to the context of the
       * characterPosition and also discards any irrelevant information that would have already been handled in a
       * previous iteration of this method.
       * 
       * For example: If we are at character position 2 then numericIndex has to be a number greater than 676
       * (which is 26 squared).  Say numericIndex is 3000 then the final result should be "dkk".  To get the "d" we
       * only care about the part of numericIndex that is a multiple of 676.
       */
      truncatedIndex = Math.Truncate((double) numericIndex / Math.Pow(alphabetLength, characterPosition));
      
      /* On any run of this method beyond the first, "a" is at position 1 of the truncatedIndex.  On the first run, "a"
       * is at position zero.  This is because the alphabet has no character to represent zero.  What we do is subtract
       * 1 from the truncated index on any run after the first (IE: for characterPosition > 0).
       */
      if(characterPosition > 0)
      {
        truncatedIndex = truncatedIndex - 1;
      }
      
      /* The index of the character we want is the remainder of the truncatedIndex divided by the length of the
       * alphabet.
       */
      character = truncatedIndex % alphabetLength;
      output.Insert(0, ALPHABET[(int) character]);
      
      // Here we decide if we need to recurse and run another iteration.
      if(truncatedIndex >= alphabetLength)
      {
        GenerateLetterReference(numericIndex, characterPosition + 1, ref output);
      }
    }
    
    #endregion
  }
}
