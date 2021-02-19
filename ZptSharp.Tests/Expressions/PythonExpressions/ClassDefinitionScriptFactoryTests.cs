using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace ZptSharp.Expressions.PythonExpressions
{
    [TestFixture,Parallelizable]
    public class ClassDefinitionScriptFactoryTests
    {
        [Test, AutoMoqData]
        public void GetScript_can_get_a_script_with_no_variables(ClassDefinitionScriptFactory sut)
        {
            var expected = @"
class PythonExpression:
    def evaluate(self, __params):
        
        return 0";
            Assert.That(() => sut.GetScript("0", new List<Variable>()).ScriptBody, Is.EqualTo(expected));
        }

        [Test, AutoMoqData]
        public void GetScript_can_get_a_script_with_one_variable(ClassDefinitionScriptFactory sut)
        {
            var expected = @"
class PythonExpression:
    def evaluate(self, __params):
        foo = __params[0]
        return foo.toString()";
            var variables = new List<Variable>
            {
                new Variable("foo", "bar"),
            };

            Assert.That(() => sut.GetScript("foo.toString()", variables).ScriptBody, Is.EqualTo(expected));
        }

        [Test, AutoMoqData]
        public void GetScript_can_get_a_script_with_multiple_variable(ClassDefinitionScriptFactory sut)
        {
            var expected = @"
class PythonExpression:
    def evaluate(self, __params):
        foo, bar, baz = __params
        return foo + bar + baz";
            var variables = new List<Variable>
            {
                new Variable("foo", 1),
                new Variable("bar", 2),
                new Variable("baz", 3),
            };

            Assert.That(() => sut.GetScript("foo + bar + baz", variables).ScriptBody, Is.EqualTo(expected));
        }
    }
}
