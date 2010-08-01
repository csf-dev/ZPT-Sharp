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
  /// <para>Represents a repeat variable for using with a TAL 'repeat' directive.</para>
  /// </summary>
  public class RepeatVariable : IEnumerator
  {
    #region constants
    
    private const string
      ALPHABET                = "abcdefghijklmnopqrstuvwxyz";
    
    private const int
      ALPHABET_LENGTH         = 26;
    
    #endregion
    
    #region fields
    
    private IList underlyingCollection;
    private int index;
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// <para>Read-only.  Gets the index of the current iteration (zero-based).</para>
    /// </summary>
    [TalesAlias("index")]
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
    [TalesAlias("number")]
    public int Number
    {
      get {
        return this.Index + 1;
      }
    }
    
    /// <summary>
    /// <para>Read-only.  Gets whether the <see cref="Index"/> indicates an even-numbered iteration.</para>
    /// </summary>
    [TalesAlias("even")]
    public bool Even
    {
      get {
        return Math.Round((float) this.Index / 2f) == ((float) this.Index / 2f);
      }
    }
    
    /// <summary>
    /// <para>Read-only.  Gets whether the <see cref="Index"/> indicates an odd-numbered iteration.</para>
    /// </summary>
    [TalesAlias("odd")]
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
    [TalesAlias("start")]
    public bool Start
    {
      get {
        return this.Index == 0;
      }
    }
    
    /// <summary>
    /// <para>
    /// Read-only.  Gets whether the <see cref="Index"/> that we are at the end of the
    /// <see cref="UnderlyingCollection"/>.
    /// </para>
    /// </summary>
    [TalesAlias("end")]
    public bool End
    {
      get {
        return this.Index == (this.Length - 1);
      }
    }
    
    /// <summary>
    /// <para>Read-only.  Gets the length of the <see cref="UnderlyingCollection"/>.</para>
    /// </summary>
    [TalesAlias("length")]
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
    [TalesAlias("letter")]
    public string Letter
    {
      get {
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
    [TalesAlias("Letter")]
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
    public object Current
    {
      get {
        if(this.Index < 0)
        {
          throw new InvalidOperationException("The index is currently positioned before the start of the collection. " +
                                              "Have you forgotten to use 'MoveNext()'?");
        }
        else if(this.Index >= this.Length)
        {
          throw new InvalidOperationException("The index is currently positioned beyond the end of the collection.");
        }
        
        return this.UnderlyingCollection[this.Index];
      }
    }
    
    /// <summary>
    /// <para>
    /// Read-only.  Gets the <see cref="IList"/> that provides the underlying collection for this instance of
    /// <see cref="RepeatVariable"/>.
    /// </para>
    /// </summary>
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
    /// A <see cref="System.Boolean"/>, if there is another item within the collection then this returns true.  If this
    /// action would place <see cref="Index"/> past the end of the <see cref="UnderlyingCollection"/> then false is
    /// returned.
    /// </returns>
    public bool MoveNext()
    {
      bool output;
      
      if(this.Number < this.Length)
      {
        this.Index ++;
        output = true;
      }
      else
      {
        output = false;
      }
      
      return output;
    }
    
    /// <summary>
    /// <para>This method should never be used and will always throw a <see cref="NotSupportedException"/>.</para>
    /// </summary>
    /// <remarks>
    /// <para>This method signature is only included as it is required by <see cref="IEnumerator"/>.</para>
    /// </remarks>
    public void Reset()
    {
      throw new NotSupportedException("Resetting the repeat variable is not supported");
    }
    
    /// <summary>
    /// <para>Generates a letter reference for a given <paramref name="indexReference"/>.</para>
    /// </summary>
    /// <example>
    /// <para>If <paramref name="indexReference"/> is <c>500</c> then this method would return <c>tg</c>.</para>
    /// </example>
    /// <param name="indexReference">
    /// A zero-based <see cref="System.Int32"/>, the numeric index for which this string reference will be generated.
    /// </param>
    /// <returns>
    /// A <see cref="System.String"/> derived from the <paramref name="indexReference"/> using the scheme <c>a</c> to
    /// <c>z</c> and then <c>aa</c> to <c>az</c>, <c>ba</c> to <c>bz</c> and so on.
    /// </returns>
    public string GenerateLetterReference(int indexReference)
    {
      StringBuilder output = new StringBuilder();
      
      GenerateLetterReference(indexReference, 1, ref output);
      
      return output.ToString();
    }
    
    private void GenerateLetterReference(int indexReference, int power, ref StringBuilder output)
    {
      double characterPosition, character;
      
      characterPosition = Math.Pow(ALPHABET_LENGTH, power);
      character = (double) indexReference % characterPosition;
      
#if DEBUG
      if(character < 0 || character > 25)
      {
        throw new InvalidOperationException(String.Format("Could not add character - requested index was {0}",
                                                          character));
      }
#endif
      
      output.Insert(0, ALPHABET[(int) character]);
      
      if(indexReference >= characterPosition)
      {
        GenerateLetterReference(indexReference, power + 1, ref output);
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
    public RepeatVariable(IList list)
    {
      this.UnderlyingCollection = list;
      this.Index = -1;
    }
    
    #endregion
  }
}
