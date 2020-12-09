using System;
using AutoFixture.NUnit3;
using NUnit.Framework;
using ZptSharp.Expressions;

namespace ZptSharp.Metal
{
    [TestFixture,Parallelizable]
    public class MetalContextProcessorFactoryTests
    {
        [Test, AutoMoqData]
        public void GetMetalContextProcessor_returns_instance([Frozen] IGetsMetalAttributeSpecs specProvider,
                                                              [Frozen] IEvaluatesExpression expressionEvaluator,
                                                              [Frozen] IExpandsMacro macroExpander,
                                                              MetalContextProcessorFactory sut)
        {
            Assert.That(() => sut.GetMetalContextProcessor(), Is.Not.Null);
        }
    }
}
