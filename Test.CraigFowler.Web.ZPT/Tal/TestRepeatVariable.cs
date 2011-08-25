//  
//  TestRepeatVariable.cs
//  
//  Author:
//       Craig Fowler <craig@craigfowler.me.uk>
// 
//  Copyright (c) 2011 Craig Fowler
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
using System.Collections.Generic;
using CraigFowler.Web.ZPT.Tal;
using NUnit.Framework;

namespace Test.CraigFowler.Web.ZPT.Tal
{
  [TestFixture]
  public class TestRepeatVariable
  {
    #region tests
    
    [Test]
    public void TestGenerateLetterReference()
    {
      Assert.AreEqual("a",   RepeatVariable.GenerateLetterReference(0),    "Letter reference for 0");
      Assert.AreEqual("g",   RepeatVariable.GenerateLetterReference(6),    "Letter reference for 6");
      Assert.AreEqual("aa",  RepeatVariable.GenerateLetterReference(26),   "Letter reference for 26");
      Assert.AreEqual("ab",  RepeatVariable.GenerateLetterReference(27),   "Letter reference for 27");
      Assert.AreEqual("sg",  RepeatVariable.GenerateLetterReference(500),  "Letter reference for 500");
      Assert.AreEqual("dkk", RepeatVariable.GenerateLetterReference(3000), "Letter reference for 3000");
    }
    
    [Test]
    public void TestEven()
    {
      List<string> testList = CreateDummyList();
      RepeatVariable repeat = new RepeatVariable(testList);
      
      repeat.MoveNext();
      Assert.IsTrue(repeat.Even, "The zero'th item in the collection is an even position");
      
      repeat.MoveNext();
      Assert.IsFalse(repeat.Even, "The first item in the collection is not an even position");
      
      repeat.MoveNext();
      Assert.IsTrue(repeat.Even, "The second item in the collection is an even position");
      
      repeat.MoveNext();
      Assert.IsFalse(repeat.Even, "The third item in the collection is not an even position");
    }
    
    [Test]
    public void TestOdd()
    {
      List<string> testList = CreateDummyList();
      RepeatVariable repeat = new RepeatVariable(testList);
      
      repeat.MoveNext();
      Assert.IsFalse(repeat.Odd, "The zero'th item in the collection is not an odd position");
      
      repeat.MoveNext();
      Assert.IsTrue(repeat.Odd, "The first item in the collection is an odd position");
      
      repeat.MoveNext();
      Assert.IsFalse(repeat.Odd, "The second item in the collection is not an odd position");
      
      repeat.MoveNext();
      Assert.IsTrue(repeat.Odd, "The third item in the collection is an odd position");
    }
    
    [Test]
    public void TestStart()
    {
      List<string> testList = CreateDummyList();
      RepeatVariable repeat = new RepeatVariable(testList);
      
      repeat.MoveNext();
      Assert.IsTrue(repeat.Start, "The zero'th item in the collection is the start element");
      
      repeat.MoveNext();
      Assert.IsFalse(repeat.Start, "The first item in the collection is not the start element");
      
      repeat.MoveNext();
      Assert.IsFalse(repeat.Start, "The second item in the collection is not the start element");
      
      repeat.MoveNext();
      Assert.IsFalse(repeat.Start, "The third item in the collection is not the start element");
    }
    
    [Test]
    public void TestEnd()
    {
      List<string> testList = CreateDummyList();
      RepeatVariable repeat = new RepeatVariable(testList);
      
      repeat.MoveNext();
      Assert.IsFalse(repeat.End, "The zero'th item in the collection is not the end element");
      
      repeat.MoveNext();
      Assert.IsFalse(repeat.End, "The first item in the collection is not the end element");
      
      repeat.MoveNext();
      Assert.IsFalse(repeat.End, "The second item in the collection is not the end element");
      
      repeat.MoveNext();
      Assert.IsFalse(repeat.End, "The third item in the collection is not the end element");
      
      repeat.MoveNext();
      Assert.IsTrue(repeat.End, "The fourth item in the collection is the end element");
    }
    
    [Test]
    public void TestLength()
    {
      List<string> testList = CreateDummyList();
      RepeatVariable repeat = new RepeatVariable(testList);
      
      Assert.AreEqual(5, repeat.Length);
    }
    
    [Test]
    public void TestLetter()
    {
      List<string> testList = CreateDummyList();
      RepeatVariable repeat = new RepeatVariable(testList);
      
      repeat.MoveNext();
      Assert.AreEqual("A", repeat.UppercaseLetter, "Uppercase letter for zero'th item");
      
      repeat.MoveNext();
      Assert.AreEqual("b", repeat.Letter, "Uppercase letter for first item");
      
      repeat.MoveNext();
      Assert.AreEqual("c", repeat.Letter, "Uppercase letter for second item");
      
      repeat.MoveNext();
      Assert.AreEqual("D", repeat.UppercaseLetter, "Uppercase letter for third item");
    }
    
    [Test]
    [ExpectedException(ExceptionType = typeof(InvalidOperationException),
                       ExpectedMessage = "The index is currently positioned before the start of the collection. " +
                                         "Have you forgotten to use 'MoveNext()'?")]
    public void TestEvenBeforeStartOfRange()
    {
      List<string> testList = CreateDummyList();
      RepeatVariable repeat = new RepeatVariable(testList);
      bool isEven;
      
      /* This line should throw an exception because we haven't used MoveNext() yet and so we are before the
       * start of the collection.
       */
      isEven = repeat.Even;
      Assert.IsInstanceOfType(typeof(Boolean),
                              isEven,
                              "Not really a test, just prevents a compiler warning for not using 'isEven'.");
    }
    
    [Test]
    [ExpectedException(ExceptionType = typeof(InvalidOperationException),
                       ExpectedMessage = "The index is currently positioned beyond the end of the collection.")]
    public void TestEvenAfterEndOfRange()
    {
      List<string> testList = CreateDummyList();
      RepeatVariable repeat = new RepeatVariable(testList);
      bool isEven;
      
      // These are enough to put us past the end of the collection.
      repeat.MoveNext();
      repeat.MoveNext();
      repeat.MoveNext();
      repeat.MoveNext();
      repeat.MoveNext();
      repeat.MoveNext();
      
      /* This line should throw an exception because we haven't used MoveNext() yet and so we are beyond the
       * end of the collection.
       */
      isEven = repeat.Even;
      Assert.IsInstanceOfType(typeof(Boolean),
                              isEven,
                              "Not really a test, just prevents a compiler warning for not using 'isEven'.");
    }
    
    [Test]
    public void TestMoveNext()
    {
      List<string> testList = CreateDummyList();
      RepeatVariable repeat = new RepeatVariable(testList);
      
      Assert.AreEqual(-1, repeat.Index, "Index before zero'th item");
      
      Assert.IsTrue(repeat.MoveNext(), "Zero'th item");
      Assert.AreEqual(0, repeat.Index, "Index at 0");
      
      Assert.IsTrue(repeat.MoveNext(), "First item");
      Assert.AreEqual(1, repeat.Index, "Index at 1");
      
      Assert.IsTrue(repeat.MoveNext(), "Second item");
      Assert.AreEqual(2, repeat.Index, "Index at 2");
      
      Assert.IsTrue(repeat.MoveNext(), "Third item");
      Assert.AreEqual(3, repeat.Index, "Index at 3");
      
      Assert.IsTrue(repeat.MoveNext(), "Fourth item");
      Assert.AreEqual(4, repeat.Index, "Index at 4");
      
      Assert.IsFalse(repeat.MoveNext(), "Have now moved beyond the end of the collection");
      Assert.AreEqual(5, repeat.Index, "Index after end of collection");
    }
    
    [Test]
    public void TestCurrent()
    {
      List<string> testList = CreateDummyList();
      RepeatVariable repeat = new RepeatVariable(testList);
      
      repeat.MoveNext();
      Assert.AreEqual("foo", repeat.Current, "The zero'th item in the collection");
      
      repeat.MoveNext();
      Assert.AreEqual("bar", repeat.Current, "The first item in the collection");
      
      repeat.MoveNext();
      Assert.AreEqual("baz", repeat.Current, "The second item in the collection");
      
      repeat.MoveNext();
      Assert.AreEqual("spong", repeat.Current, "The third item in the collection");
      
      repeat.MoveNext();
      Assert.AreEqual("wibble", repeat.Current, "The fourth item in the collection");
    }
    
    #endregion
    
    #region helper methods
    
    /// <summary>
    /// <para>Helper method that creates and returns a collection of 5 <see cref="System.String"/> instances.</para>
    /// </summary>
    /// <returns>
    /// A collection of <see cref="System.String"/> with exactly 5 elements.
    /// </returns>
    private List<string> CreateDummyList()
    {
      List<string> output = new List<string>();
      
      output.Add("foo");
      output.Add("bar");
      output.Add("baz");
      output.Add("spong");
      output.Add("wibble");
      
      return output;
    }
    
    #endregion
  }
}
