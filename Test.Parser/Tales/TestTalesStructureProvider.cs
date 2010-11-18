using System;
using NUnit.Framework;
using CraigFowler.Web.ZPT.Tales;

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

