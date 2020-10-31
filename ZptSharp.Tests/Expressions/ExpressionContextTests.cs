using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace ZptSharp.Expressions
{
    [TestFixture,Parallelizable]
    public class ExpressionContextTests
    {
        [Test, AutoMoqData]
        public void Constructor_should_clone_local_definitions(IDictionary<string,object> localDefs)
        {
            var result = new ExpressionContext(localDefinitions: localDefs);
            Assert.That(result.LocalDefinitions, Is.EqualTo(localDefs), "Collections are equal");
            Assert.That(result.LocalDefinitions, Is.Not.SameAs(localDefs), "Collections are not the same instance");
        }

        [Test, AutoMoqData]
        public void Constructor_should_clone_repetition_info(IDictionary<string, RepetitionInfo> repetitions)
        {
            var result = new ExpressionContext(repetitions: repetitions);
            Assert.That(result.Repetitions, Is.EqualTo(repetitions), "Collections are equal");
            Assert.That(result.Repetitions, Is.Not.SameAs(repetitions), "Collections are not the same instance");
        }

        [Test, AutoMoqData]
        public void Constructor_should_use_same_collection_for_global_definitions(IDictionary<string, object> globalDefs)
        {
            var result = new ExpressionContext(globalDefinitions: globalDefs);
            Assert.That(result.GlobalDefinitions, Is.SameAs(globalDefs), "Collections are the same instance");
        }
    }
}
