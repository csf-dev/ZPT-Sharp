//  
//  CSharpEvaluatorProofOfConcept.cs
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
using CraigFowler.Web.ZPT.Mocks;
using Mono.CSharp;
using NUnit.Framework;

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
