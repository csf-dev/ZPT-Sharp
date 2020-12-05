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
        public void Constructor_should_clone_local_definitions(INode element, IDictionary<string,object> localDefs)
        {
            var result = new ExpressionContext(element, localDefs, null, null, null);
            Assert.That(result.LocalDefinitions, Is.EqualTo(localDefs), "Collections are equal");
            Assert.That(result.LocalDefinitions, Is.Not.SameAs(localDefs), "Collections are not the same instance");
        }

        [Test, AutoMoqData]
        public void Constructor_should_clone_repetition_info(INode element, IDictionary<string, RepetitionInfo> repetitions)
        {
            var result = new ExpressionContext(element, null, null, repetitions, null);
            Assert.That(result.Repetitions, Is.EqualTo(repetitions), "Collections are equal");
            Assert.That(result.Repetitions, Is.Not.SameAs(repetitions), "Collections are not the same instance");
        }

        [Test, AutoMoqData]
        public void Constructor_should_clone_error_handlers_info(INode element, Stack<ErrorHandlingContext> errorHandlers)
        {
            var result = new ExpressionContext(element, null, null, null, errorHandlers);
            Assert.That(result.ErrorHandlers, Is.EqualTo(errorHandlers), "Collections are equal");
            Assert.That(result.ErrorHandlers, Is.Not.SameAs(errorHandlers), "Collections are not the same instance");
        }

        [Test, AutoMoqData]
        public void Constructor_should_use_same_collection_for_global_definitions(INode element, IDictionary<string, object> globalDefs)
        {
            var result = new ExpressionContext(element, null, globalDefs, null, null);
            Assert.That(result.GlobalDefinitions, Is.SameAs(globalDefs), "Collections are the same instance");
        }

        [Test, AutoMoqData]
        public void Constructor_throws_ANE_if_element_is_null()
        {
            Assert.That(() => new ExpressionContext(null), Throws.ArgumentNullException);
        }

        [Test, AutoMoqData]
        public void CreateChild_should_clone_local_definitions(ExpressionContext context, INode element)
        {
            var result = context.CreateChild(element);
            Assert.That(result.LocalDefinitions, Is.EqualTo(context.LocalDefinitions), "Collections are equal");
            Assert.That(result.LocalDefinitions, Is.Not.SameAs(context.LocalDefinitions), "Collections are not the same instance");
        }

        [Test, AutoMoqData]
        public void CreateChild_should_clone_repetition_info(ExpressionContext context, INode element)
        {
            var result = context.CreateChild(element);
            Assert.That(result.Repetitions, Is.EqualTo(context.Repetitions), "Collections are equal");
            Assert.That(result.Repetitions, Is.Not.SameAs(context.Repetitions), "Collections are not the same instance");
        }

        [Test, AutoMoqData]
        public void CreateChild_should_use_same_collection_for_global_definitions(ExpressionContext context, INode element)
        {
            var result = context.CreateChild(element);
            Assert.That(result.GlobalDefinitions, Is.SameAs(context.GlobalDefinitions), "Collections are not the same instance");
        }

        [Test, AutoMoqData]
        public void CreateChild_throws_ANE_if_element_is_null(ExpressionContext context)
        {
            Assert.That(() => context.CreateChild(null), Throws.ArgumentNullException);
        }
    }
}
