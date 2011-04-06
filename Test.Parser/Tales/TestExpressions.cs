
using CraigFowler.Web.ZPT.Mocks;
using CraigFowler.Web.ZPT.Tales;
using CraigFowler.Web.ZPT.Tales.Exceptions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Test.CraigFowler.Web.ZPT.Tales
{
	[TestFixture]
	[Description("These are a batch of tests imported from the Zope source code.")]
	[Category("Integration")]
	public class TestExpressions
	{
		#region private variables
		
		private TalesTestObject testObject;
		private TalesContext context;
		
		#endregion
		
		#region set up
		
		[SetUp]
		public void SetUpTests()
		{
			// Re-create the context using a new test object.
			testObject = new TalesTestObject();
			context = testObject.CreateContext();
			context.AddDefinition("someClass", typeof(MockObject));
		}
		
		#endregion
		
		#region path expression tests
		
		[Test]
		public void TestSimple()
		{
			TalesExpression expression = context.CreateExpression("x");
			Assert.AreEqual(testObject["x"],
			                expression.GetValue());
		}
		
		[Test]
		public void TestPath()
		{
			TalesExpression expression = context.CreateExpression("x/y");
			Assert.AreEqual(((Dictionary<string, object>)testObject["x"])["y"],
			                expression.GetValue());
		}
		
		[Test]
		public void TestLongPath()
		{
			TalesExpression expression = context.CreateExpression("x/y/z");
			Assert.AreEqual(((Dictionary<string, object>)((Dictionary<string, object>)testObject["x"])["y"])["z"],
			                expression.GetValue());
		}
		
		[Test]
		public void TestOrPath()
		{
			TalesExpression expression = context.CreateExpression("path:a|b|c/d/e");
			Assert.AreEqual(testObject["b"],
			                expression.GetValue());
			
			foreach(string exceptionType in TalesTestObject.ExceptionThrower.ExceptionTypes)
			{
				expression = context.CreateExpression(String.Format("path:ErrorGenerator/{0}|b|c/d/e", exceptionType));
				Assert.AreEqual(testObject["b"],
				                expression.GetValue());
			}
		}
		
		[Test]
		public void TestDynamic()
		{
			TalesExpression expression = context.CreateExpression("x/y/?dynamic");
			
			try
			{
				Assert.AreEqual(((Dictionary<string, object>)((Dictionary<string, object>)testObject["x"])["y"])["z"],
				                expression.GetValue());
			}
			catch(TraversalException ex)
			{
				foreach(TalesPath path in ex.Attempts.Keys)
				{
					Console.WriteLine ("Path: '{0}', Exception: {1}", path.ToString(), ex.Attempts[path].ToString());
				}
				
				throw;
			}
		}
		
		[Test]
		[ExpectedException(ExceptionType = typeof(PathInvalidException))]
		public void TestBadInitialDynamic()
		{
			TalesExpression expression = context.CreateExpression("?x");
			Assert.IsNull(expression,
			              "Not a real assert, just here to prevent a compiler warning for not using 'expression'.");
		}
		
		#endregion
		
		#region string expression tests
		
		[Test]
		public void TestString()
		{
			TalesExpression expression = context.CreateExpression("string:Fred");
			Assert.AreEqual("Fred", expression.GetValue());
		}
		
		[Test]
		public void TestStringSub()
		{
			TalesExpression expression = context.CreateExpression("string:A$B");
			Assert.AreEqual("A2", expression.GetValue());
		}
		
		[Test]
		public void TestStringSubComplex()
		{
			TalesExpression expression = context.CreateExpression("string:a ${x/y/name} b ${y/z} c");
			Assert.AreEqual("a yikes b 3 c", expression.GetValue());
		}
		
		#endregion
		
		#region hybrid path tests
		
		public void TestHybridPathExpressions()
		{
			TalesContext context = new TalesContext();
			context.AddDefinition("one", 1);
			
			Assert.AreEqual("x", context.CreateExpression("foo | string:x").GetValue());
			Assert.AreEqual(1, context.CreateExpression("foo | string:$one").GetValue());
		}
		
		#endregion
		
		#region invalid expressions
		
		[Test]
		public void TestInvalidExpressions()
		{
			string[] invalidExpressions = new string[] {	"/ab/cd | c/d | e/f",
																										"ab//cd | c/d | e/f",
																										"/ab/cd | c/d | e/f",
																										"ab//cd | c/d | e/f",
																										"ab/cd/ | c/d | e/f",
																										"ab/cd | /c/d | e/f",
																										"ab/cd | c//d | e/f",
																										"ab/cd | c/d/ | e/f",
																										"ab/cd | c/d | /e/f",
																										"ab/cd | c/d | e//f",
																										"ab/cd | c/d | e/f/",
																										"string:${/ab/cd}",
																										"string:${ab//cd}",
																										"string:${ab/cd/}",
																										"string:foo${/ab/cd | c/d | e/f}bar",
																										"string:foo${ab//cd | c/d | e/f}bar",
																										"string:foo${ab/cd/ | c/d | e/f}bar",
																										"string:foo${ab/cd | /c/d | e/f}bar",
																										"string:foo${ab/cd | c//d | e/f}bar",
																										"string:foo${ab/cd | c/d/ | e/f}bar",
																										"string:foo${ab/cd | c/d | /e/f}bar",
																										"string:foo${ab/cd | c/d | e//f}bar",
																										"string:foo${ab/cd | c/d | e/f/}bar",
																										"/ab/cd | c/d | e/f",
																										"ab//cd | c/d | e/f",
																										"ab/cd/ | c/d | e/f",
																										"ab/cd | /c/d | e/f",
																										"ab/cd | c//d | e/f",
																										"ab/cd | c/d/ | e/f",
																										"ab/cd | c/d | /e/f",
																										"ab/cd | c/d | e//f",
																										"ab/cd | c/d | e/f/" };
			
			foreach(string expressionText in invalidExpressions)
			{
				try
				{
					TalesExpression expression = context.CreateExpression(expressionText);
					object expressionValue = expression.GetValue();
					Assert.Fail(String.Format("An invalid expression was accepted: '{0}' and was evaluated to '{1}'",
					                          expressionText,
					                          expressionValue));
				}
				catch(TalesException) {}
				catch(FormatException) {}
			}

		}
		
		#endregion
	}
}
