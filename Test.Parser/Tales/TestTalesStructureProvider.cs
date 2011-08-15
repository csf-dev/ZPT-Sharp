//  
//  TestTalesStructureProvider.cs
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

using CraigFowler.Web.ZPT.Tales;
using NUnit.Framework;

namespace Test.CraigFowler.Web.ZPT.Tales
{
	[TestFixture]
	public class TestTalesStructureProvider
	{
		[Test]
		public void TestStoreItem()
		{
			TalesStructureProvider
				provider = new TalesStructureProvider(),
				levelOne,
				levelTwo;
			TalesPath path = new TalesPath("test/foo/bar");
			object item = "This is a sample object";
			
			provider.StoreItem(path, item);
			levelOne = (TalesStructureProvider) provider["test"];
			levelTwo = (TalesStructureProvider) levelOne["foo"];
			
			Assert.AreEqual("This is a sample object", levelTwo["bar"], "Evaluate correctly into the provider");
      
      Assert.AreEqual("This is a sample object",
                      ((TalesStructureProvider) ((TalesStructureProvider) provider["test"])["foo"])["bar"],
                      "Evaluates correctly using explicit casts");
		}
		
		[Test]
		[Category("Integration")]
		public void TestStoreItemIntegrationTest()
		{
			TalesStructureProvider provider = new TalesStructureProvider();
			TalesPath path = new TalesPath("test/foo/bar");
			object item = "This is a sample object";
			TalesContext context = new TalesContext();
			TalesExpression expression;
			
			provider.StoreItem(path, item);
			context.AddDefinition("provider", provider);
			expression = context.CreateExpression("provider/test/foo/bar");
			
			Assert.AreEqual("This is a sample object",
			                expression.GetValue(),
			                "TALES expression evaluates correctly through the provider");
		}
		
    [Test]
    public void TestRemoveItem()
    {
      TalesStructureProvider
        provider = new TalesStructureProvider(),
        levelOne,
        levelTwo;
      TalesPath path = new TalesPath("test/foo/bar");
      object item = "This is a sample object";
      
      provider.StoreItem(path, item);
      levelOne = (TalesStructureProvider) provider["test"];
      levelTwo = (TalesStructureProvider) levelOne["foo"];
      
      Assert.IsTrue(levelTwo.ContainsKey("bar"), "Level two path component contains an item.");
      
      provider.RemoveItem(path);
      
      Assert.IsFalse(levelTwo.ContainsKey("bar"), "Level two path component no longer contains an item.");
    }
	}
}

