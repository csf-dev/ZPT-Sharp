using System;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Dom;

namespace ZptSharp.Tal
{
    [TestFixture,Parallelizable]
    public class TalAttributeSpecProviderTests
    {
        [Test, AutoMoqData]
        public void Attributes_returns_instance_with_correct_local_name([Frozen] IGetsWellKnownNamespace namespaceProvider,
                                                                        Namespace @namespace,
                                                                        TalAttributeSpecProvider sut)
        {
            Mock.Get(namespaceProvider).SetupGet(x => x.TalNamespace).Returns(@namespace);
            Assert.That(() => sut.Attributes?.Name, Is.EqualTo("attributes"));
        }

        [Test, AutoMoqData]
        public void Condition_returns_instance_with_correct_local_name([Frozen] IGetsWellKnownNamespace namespaceProvider,
                                                                        Namespace @namespace,
                                                                        TalAttributeSpecProvider sut)
        {
            Mock.Get(namespaceProvider).SetupGet(x => x.TalNamespace).Returns(@namespace);
            Assert.That(() => sut.Condition?.Name, Is.EqualTo("condition"));
        }

        [Test, AutoMoqData]
        public void Content_returns_instance_with_correct_local_name([Frozen] IGetsWellKnownNamespace namespaceProvider,
                                                                        Namespace @namespace,
                                                                        TalAttributeSpecProvider sut)
        {
            Mock.Get(namespaceProvider).SetupGet(x => x.TalNamespace).Returns(@namespace);
            Assert.That(() => sut.Content?.Name, Is.EqualTo("content"));
        }

        [Test, AutoMoqData]
        public void Replace_returns_instance_with_correct_local_name([Frozen] IGetsWellKnownNamespace namespaceProvider,
                                                                        Namespace @namespace,
                                                                        TalAttributeSpecProvider sut)
        {
            Mock.Get(namespaceProvider).SetupGet(x => x.TalNamespace).Returns(@namespace);
            Assert.That(() => sut.Replace?.Name, Is.EqualTo("replace"));
        }

        [Test, AutoMoqData]
        public void Define_returns_instance_with_correct_local_name([Frozen] IGetsWellKnownNamespace namespaceProvider,
                                                                        Namespace @namespace,
                                                                        TalAttributeSpecProvider sut)
        {
            Mock.Get(namespaceProvider).SetupGet(x => x.TalNamespace).Returns(@namespace);
            Assert.That(() => sut.Define?.Name, Is.EqualTo("define"));
        }

        [Test, AutoMoqData]
        public void OmitTag_returns_instance_with_correct_local_name([Frozen] IGetsWellKnownNamespace namespaceProvider,
                                                                        Namespace @namespace,
                                                                        TalAttributeSpecProvider sut)
        {
            Mock.Get(namespaceProvider).SetupGet(x => x.TalNamespace).Returns(@namespace);
            Assert.That(() => sut.OmitTag?.Name, Is.EqualTo("omit-tag"));
        }

        [Test, AutoMoqData]
        public void OnError_returns_instance_with_correct_local_name([Frozen] IGetsWellKnownNamespace namespaceProvider,
                                                                        Namespace @namespace,
                                                                        TalAttributeSpecProvider sut)
        {
            Mock.Get(namespaceProvider).SetupGet(x => x.TalNamespace).Returns(@namespace);
            Assert.That(() => sut.OnError?.Name, Is.EqualTo("on-error"));
        }

        [Test, AutoMoqData]
        public void Repeat_returns_instance_with_correct_local_name([Frozen] IGetsWellKnownNamespace namespaceProvider,
                                                                        Namespace @namespace,
                                                                        TalAttributeSpecProvider sut)
        {
            Mock.Get(namespaceProvider).SetupGet(x => x.TalNamespace).Returns(@namespace);
            Assert.That(() => sut.Repeat?.Name, Is.EqualTo("repeat"));
        }
    }
}
