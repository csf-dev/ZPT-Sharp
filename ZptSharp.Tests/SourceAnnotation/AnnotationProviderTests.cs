using System;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Dom;
using ZptSharp.Rendering;

namespace ZptSharp.SourceAnnotation
{
    [TestFixture,Parallelizable]
    public class AnnotationProviderTests
    {
        [Test, AutoMoqData]
        public void GetAnnotation_gets_annotation_with_element_source_info_and_start_tag([Frozen] IGetsSourceAnnotationString sourceInfoProvider,
                                                                                         AnnotationProvider sut,
                                                                                         INode node,
                                                                                         ElementSourceInfo sourceInfo,
                                                                                         string annotation)
        {
            Mock.Get(node).SetupGet(x => x.SourceInfo).Returns(sourceInfo);
            Mock.Get(sourceInfoProvider).Setup(x => x.GetStartTagInfo(sourceInfo)).Returns(annotation);

            var result = sut.GetAnnotation(node, TagType.Start);

            Assert.That(result, Is.EqualTo($@"
==============================================================================
{annotation}
==============================================================================
"));
        }

        [Test, AutoMoqData]
        public void GetAnnotation_gets_annotation_with_element_source_info_and_end_tag([Frozen] IGetsSourceAnnotationString sourceInfoProvider,
                                                                                         AnnotationProvider sut,
                                                                                         INode node,
                                                                                         ElementSourceInfo sourceInfo,
                                                                                         string annotation)
        {
            Mock.Get(node).SetupGet(x => x.SourceInfo).Returns(sourceInfo);
            Mock.Get(sourceInfoProvider).Setup(x => x.GetEndTagInfo(sourceInfo)).Returns(annotation);

            var result = sut.GetAnnotation(node, TagType.End);

            Assert.That(result, Is.EqualTo($@"
==============================================================================
{annotation}
==============================================================================
"));
        }

        [Test, AutoMoqData]
        public void GetAnnotation_gets_annotation_with_element_source_info_and_no_tag_info([Frozen] IGetsSourceAnnotationString sourceInfoProvider,
                                                                                           AnnotationProvider sut,
                                                                                           INode node,
                                                                                           ElementSourceInfo sourceInfo,
                                                                                           string annotation)
        {
            Mock.Get(node).SetupGet(x => x.SourceInfo).Returns(sourceInfo);
            Mock.Get(sourceInfoProvider).Setup(x => x.GetSourceInfo(sourceInfo.Document)).Returns(annotation);

            var result = sut.GetAnnotation(node, TagType.None);

            Assert.That(result, Is.EqualTo($@"
==============================================================================
{annotation}
==============================================================================
"));
        }
        [Test, AutoMoqData]
        public void GetPreReplacementAnnotation_gets_annotation_with_pre_replacement_source_info_and_start_tag([Frozen] IGetsSourceAnnotationString sourceInfoProvider,
                                                                                                               AnnotationProvider sut,
                                                                                                               INode node,
                                                                                                               ElementSourceInfo sourceInfo,
                                                                                                               string annotation)
        {
            Mock.Get(node).SetupGet(x => x.PreReplacementSourceInfo).Returns(sourceInfo);
            Mock.Get(sourceInfoProvider).Setup(x => x.GetStartTagInfo(sourceInfo)).Returns(annotation);

            var result = sut.GetPreReplacementAnnotation(node, TagType.Start);

            Assert.That(result, Is.EqualTo($@"
==============================================================================
{annotation}
==============================================================================
"));
        }

        [Test, AutoMoqData]
        public void GetPreReplacementAnnotation_gets_annotation_with_pre_replacement_source_info_and_end_tag([Frozen] IGetsSourceAnnotationString sourceInfoProvider,
                                                                                                             AnnotationProvider sut,
                                                                                                             INode node,
                                                                                                             ElementSourceInfo sourceInfo,
                                                                                                             string annotation)
        {
            Mock.Get(node).SetupGet(x => x.PreReplacementSourceInfo).Returns(sourceInfo);
            Mock.Get(sourceInfoProvider).Setup(x => x.GetEndTagInfo(sourceInfo)).Returns(annotation);

            var result = sut.GetPreReplacementAnnotation(node, TagType.End);

            Assert.That(result, Is.EqualTo($@"
==============================================================================
{annotation}
==============================================================================
"));
        }

        [Test, AutoMoqData]
        public void GetPreReplacementAnnotation_gets_annotation_with_pre_replacement_source_info_and_no_tag_info([Frozen] IGetsSourceAnnotationString sourceInfoProvider,
                                                                                                                 AnnotationProvider sut,
                                                                                                                 INode node,
                                                                                                                 ElementSourceInfo sourceInfo,
                                                                                                                 string annotation)
        {
            Mock.Get(node).SetupGet(x => x.PreReplacementSourceInfo).Returns(sourceInfo);
            Mock.Get(sourceInfoProvider).Setup(x => x.GetSourceInfo(sourceInfo.Document)).Returns(annotation);

            var result = sut.GetPreReplacementAnnotation(node, TagType.None);

            Assert.That(result, Is.EqualTo($@"
==============================================================================
{annotation}
==============================================================================
"));
        }
    }
}
