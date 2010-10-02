using System;
using NUnit.Framework;
using Mono.CSharp;
using CraigFowler.Web.ZPT.Mocks;

namespace Test.CraigFowler.Web.ZPT.Tales.Expressions
{
	[TestFixture]
	public class CSharpEvaluatorProofOfConcept
	{
		#region static field
		
		[ThreadStatic]
		public static object MockObject;
		
		#endregion
		
		#region proof of concepts
		
		[Test]
		public void TestBooleanExpression()
		{
			string expression = "1 == 1;";
			object output;
			
			output = Evaluator.Evaluate(expression);
			Assert.IsInstanceOfType(typeof(bool), output, "Correct type");
			Assert.IsTrue((bool) output, "Output is truth");
		}
		
		[Test]
		public void TestStaticVariable()
		{
			string expression = @"((MockObject) CSharpEvaluatorProofOfConcept.MockObject)[""foo""] == ""bar"";";
			object output;
			
			CSharpEvaluatorProofOfConcept.MockObject = new MockObject();
			
			Evaluator.ReferenceAssembly(System.Reflection.Assembly.GetExecutingAssembly());
			Evaluator.Run("using Test.CraigFowler.Web.ZPT.Tales.Expressions;");
			Evaluator.Run("using CraigFowler.Web.ZPT.Mocks;");
			
			output = Evaluator.Evaluate(expression);
			Assert.IsInstanceOfType(typeof(bool), output, "Correct type");
			Assert.IsTrue((bool) output, "Output is truth");
		}
		
		#endregion
	}
}
