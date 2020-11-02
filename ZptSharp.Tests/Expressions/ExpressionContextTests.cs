using System;
using System.Collections.Generic;
using NUnit.Framework;
using ZptSharp.Dom;

namespace ZptSharp.Expressions
{
    [TestFixture,Parallelizable]
    public class ExpressionContextTests
    {
        [Test, AutoMoqData]
        public void Constructor_should_clone_local_definitions(IElement element, IDictionary<string,object> localDefs)
        {
            var result = new ExpressionContext(element, localDefinitions: localDefs);
            Assert.That(result.LocalDefinitions, Is.EqualTo(localDefs), "Collections are equal");
            Assert.That(result.LocalDefinitions, Is.Not.SameAs(localDefs), "Collections are not the same instance");
        }

        [Test, AutoMoqData]
        public void Constructor_should_clone_repetition_info(IElement element, IDictionary<string, RepetitionInfo> repetitions)
        {
            var result = new ExpressionContext(element, repetitions: repetitions);
            Assert.That(result.Repetitions, Is.EqualTo(repetitions), "Collections are equal");
            Assert.That(result.Repetitions, Is.Not.SameAs(repetitions), "Collections are not the same instance");
        }

        [Test, AutoMoqData]
        public void Constructor_should_use_same_collection_for_global_definitions(IElement element, IDictionary<string, object> globalDefs)
        {
            var result = new ExpressionContext(element, globalDefinitions: globalDefs);
            Assert.That(result.GlobalDefinitions, Is.SameAs(globalDefs), "Collections are the same instance");
        }

        [Test, AutoMoqData]
        public void Constructor_throws_ANE_if_element_is_null()
        {
            Assert.That(() => new ExpressionContext(null), Throws.ArgumentNullException);
        }
    }
}
