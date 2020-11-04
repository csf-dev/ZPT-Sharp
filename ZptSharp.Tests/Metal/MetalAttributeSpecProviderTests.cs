using System;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Dom;

namespace ZptSharp.Metal
{
    [TestFixture,Parallelizable]
    public class MetalAttributeSpecProviderTests
    {
        [Test, AutoMoqData]
        public void DefineMacro_returns_instance_with_correct_local_name([Frozen] IGetsWellKnownNamespace namespaceProvider,
                                                                         Namespace @namespace,
                                                                         MetalAttributeSpecProvider sut)
        {
            Mock.Get(namespaceProvider).SetupGet(x => x.MetalNamespace).Returns(@namespace);
            Assert.That(() => sut.DefineMacro?.Name, Is.EqualTo("define-macro"));
        }

        [Test, AutoMoqData]
        public void DefineMacro_returns_instance_with_correct_namespace([Frozen] IGetsWellKnownNamespace namespaceProvider,
                                                                        Namespace @namespace,
                                                                        MetalAttributeSpecProvider sut)
        {
            Mock.Get(namespaceProvider).SetupGet(x => x.MetalNamespace).Returns(@namespace);
            Assert.That(() => sut.DefineMacro?.Namespace, Is.SameAs(@namespace));
        }

        [Test, AutoMoqData]
        public void ExtendMacro_returns_instance_with_correct_local_name([Frozen] IGetsWellKnownNamespace namespaceProvider,
                                                                         Namespace @namespace,
                                                                         MetalAttributeSpecProvider sut)
        {
            Mock.Get(namespaceProvider).SetupGet(x => x.MetalNamespace).Returns(@namespace);
            Assert.That(() => sut.ExtendMacro?.Name, Is.EqualTo("extend-macro"));
        }

        [Test, AutoMoqData]
        public void ExtendMacro_returns_instance_with_correct_namespace([Frozen] IGetsWellKnownNamespace namespaceProvider,
                                                                        Namespace @namespace,
                                                                        MetalAttributeSpecProvider sut)
        {
            Mock.Get(namespaceProvider).SetupGet(x => x.MetalNamespace).Returns(@namespace);
            Assert.That(() => sut.ExtendMacro?.Namespace, Is.SameAs(@namespace));
        }

        [Test, AutoMoqData]
        public void UseMacro_returns_instance_with_correct_local_name([Frozen] IGetsWellKnownNamespace namespaceProvider,
                                                                      Namespace @namespace,
                                                                      MetalAttributeSpecProvider sut)
        {
            Mock.Get(namespaceProvider).SetupGet(x => x.MetalNamespace).Returns(@namespace);
            Assert.That(() => sut.UseMacro?.Name, Is.EqualTo("use-macro"));
        }

        [Test, AutoMoqData]
        public void UseMacro_returns_instance_with_correct_namespace([Frozen] IGetsWellKnownNamespace namespaceProvider,
                                                                     Namespace @namespace,
                                                                     MetalAttributeSpecProvider sut)
        {
            Mock.Get(namespaceProvider).SetupGet(x => x.MetalNamespace).Returns(@namespace);
            Assert.That(() => sut.UseMacro?.Namespace, Is.SameAs(@namespace));
        }

        [Test, AutoMoqData]
        public void DefineSlot_returns_instance_with_correct_local_name([Frozen] IGetsWellKnownNamespace namespaceProvider,
                                                                        Namespace @namespace,
                                                                        MetalAttributeSpecProvider sut)
        {
            Mock.Get(namespaceProvider).SetupGet(x => x.MetalNamespace).Returns(@namespace);
            Assert.That(() => sut.DefineSlot?.Name, Is.EqualTo("define-slot"));
        }

        [Test, AutoMoqData]
        public void DefineSlot_returns_instance_with_correct_namespace([Frozen] IGetsWellKnownNamespace namespaceProvider,
                                                                         Namespace @namespace,
                                                                         MetalAttributeSpecProvider sut)
        {
            Mock.Get(namespaceProvider).SetupGet(x => x.MetalNamespace).Returns(@namespace);
            Assert.That(() => sut.DefineSlot?.Namespace, Is.SameAs(@namespace));
        }

        [Test, AutoMoqData]
        public void FillSlot_returns_instance_with_correct_local_name([Frozen] IGetsWellKnownNamespace namespaceProvider,
                                                                      Namespace @namespace,
                                                                      MetalAttributeSpecProvider sut)
        {
            Mock.Get(namespaceProvider).SetupGet(x => x.MetalNamespace).Returns(@namespace);
            Assert.That(() => sut.FillSlot?.Name, Is.EqualTo("fill-slot"));
        }

        [Test, AutoMoqData]
        public void FillSlot_returns_instance_with_correct_namespace([Frozen] IGetsWellKnownNamespace namespaceProvider,
                                                                     Namespace @namespace,
                                                                     MetalAttributeSpecProvider sut)
        {
            Mock.Get(namespaceProvider).SetupGet(x => x.MetalNamespace).Returns(@namespace);
            Assert.That(() => sut.FillSlot?.Namespace, Is.SameAs(@namespace));
        }
    }
}
