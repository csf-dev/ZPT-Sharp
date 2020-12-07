using System;
using NUnit.Framework;

namespace ZptSharp.Expressions.PythonExpressions
{
    [TestFixture,Parallelizable]
    public class ClassDefinitionScriptTests
    {
        [Test, AutoMoqData]
        public void Equals_returns_true_if_name_and_script_body_are_equal(string name, string body, object result)
        {
            var first = new ClassDefinitionScript(name, body, (o, v) => result);
            var second = new ClassDefinitionScript(name, body, (o, v) => result);

            Assert.That(() => first.Equals(second), Is.True);
        }

        [Test, AutoMoqData]
        public void Equals_returns_true_if_objects_are_reference_equal(string name, string body, object result)
        {
            var first = new ClassDefinitionScript(name, body, (o, v) => result);

#pragma warning disable RECS0088 // Comparing equal expression for equality is usually useless
            Assert.That(() => first.Equals(first), Is.True);
#pragma warning restore RECS0088 // Comparing equal expression for equality is usually useless
        }

        [Test, AutoMoqData]
        public void Equals_returns_false_for_null(string name, string body, object result)
        {
            var first = new ClassDefinitionScript(name, body, (o, v) => result);

            Assert.That(() => first.Equals(null), Is.False);
        }

        [Test, AutoMoqData]
        public void Equals_returns_false_for_an_object_of_different_type(string name, string body, object result, object other)
        {
            var first = new ClassDefinitionScript(name, body, (o, v) => result);

            Assert.That(() => first.Equals(other), Is.False);
        }

        [Test, AutoMoqData]
        public void Equals_returns_false_if_name_differs(string body, object result)
        {
            var first = new ClassDefinitionScript("foo", body, (o, v) => result);
            var second = new ClassDefinitionScript("bar", body, (o, v) => result);

            Assert.That(() => first.Equals(second), Is.False);
        }

        [Test, AutoMoqData]
        public void Equals_returns_false_if_body_differs(string name, object result)
        {
            var first = new ClassDefinitionScript(name, "foo", (o, v) => result);
            var second = new ClassDefinitionScript(name, "bar", (o, v) => result);

            Assert.That(() => first.Equals(second), Is.False);
        }

        [Test, AutoMoqData]
        public void GetHashCode_returns_hash_of_name_and_body(string name, string body, object result)
        {
            var sut = new ClassDefinitionScript(name, body, (o, v) => result);
            Assert.That(() => sut.GetHashCode(), Is.EqualTo(name.GetHashCode() ^ body.GetHashCode()));
        }
    }
}
