using System;
using NUnit.Framework;

namespace ZptSharp.Rendering
{
    [TestFixture,Parallelizable]
    public class IterativeExpressionContextProcessorFactoryTests
    {
        [Test, AutoMoqData]
        public void GetContextIterator_returns_instance(IProcessesExpressionContext processor, IterativeExpressionContextProcessorFactory sut)
        {
            Assert.That(() => sut.GetContextIterator(processor), Is.InstanceOf<ExpressionContextIterativeProcessor>());
        }
    }
}
