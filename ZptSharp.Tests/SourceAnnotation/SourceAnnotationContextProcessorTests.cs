using System;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Dom;
using ZptSharp.Expressions;
using ZptSharp.Metal;
using ZptSharp.Rendering;

namespace ZptSharp.SourceAnnotation
{
    [TestFixture,Parallelizable]
    public class SourceAnnotationContextProcessorTests
    {
        [Test, AutoMoqData]
        public void ProcessContextAsync_adds_annotation_before_root_element_with_no_tag_info([Frozen] IGetsAnnotationForElement annotationProvider,
                                                                                             [Frozen] IAddsComment commenter,
                                                                                             SourceAnnotationContextProcessor sut,
                                                                                             ExpressionContext context,
                                                                                             string annotation)
        {
            context.IsRootContext = true;
            Mock.Get(annotationProvider)
                .Setup(x => x.GetAnnotation(context.CurrentElement, TagType.None))
                .Returns(annotation);

            sut.ProcessContextAsync(context);

            Mock.Get(commenter)
                .Verify(x => x.AddCommentBefore(context.CurrentElement, annotation), Times.Once);
        }

        [Test, AutoMoqData]
        public void ProcessContextAsync_adds_annotation_before_imported_element_for_start_tag([Frozen] IGetsAnnotationForElement annotationProvider,
                                                                                              [Frozen] IAddsComment commenter,
                                                                                              SourceAnnotationContextProcessor sut,
                                                                                              ExpressionContext context,
                                                                                              string annotation)
        {
            context.IsRootContext = false;
            Mock.Get(context.CurrentElement).SetupGet(x => x.IsImported).Returns(true);
            Mock.Get(annotationProvider)
                .Setup(x => x.GetAnnotation(context.CurrentElement, TagType.Start))
                .Returns(annotation);

            sut.ProcessContextAsync(context);

            Mock.Get(commenter)
                .Verify(x => x.AddCommentBefore(context.CurrentElement, annotation), Times.Once);
        }

        [Test, AutoMoqData]
        public void ProcessContextAsync_adds_annotation_after_imported_element_for_end_tag_with_pre_replace_source([Frozen] IGetsAnnotationForElement annotationProvider,
                                                                                                                   [Frozen] IAddsComment commenter,
                                                                                                                   SourceAnnotationContextProcessor sut,
                                                                                                                   ExpressionContext context,
                                                                                                                   string annotation)
        {
            context.IsRootContext = false;
            Mock.Get(context.CurrentElement).SetupGet(x => x.IsImported).Returns(true);
            Mock.Get(annotationProvider)
                .Setup(x => x.GetPreReplacementAnnotation(context.CurrentElement, TagType.End))
                .Returns(annotation);

            sut.ProcessContextAsync(context);

            Mock.Get(commenter)
                .Verify(x => x.AddCommentAfter(context.CurrentElement, annotation), Times.Once);
        }

        [Test, AutoMoqData]
        public void ProcessContextAsync_adds_annotation_before_define_macro_for_start_tag([Frozen] IGetsAnnotationForElement annotationProvider,
                                                                                          [Frozen] IAddsComment commenter,
                                                                                          [Frozen] IGetsMetalAttributeSpecs metalSpecProvider,
                                                                                          SourceAnnotationContextProcessor sut,
                                                                                          AttributeSpec spec,
                                                                                          IAttribute attr,
                                                                                          ExpressionContext context,
                                                                                          string annotation)
        {
            context.IsRootContext = false;
            Mock.Get(context.CurrentElement).SetupGet(x => x.IsImported).Returns(false);
            Mock.Get(context.CurrentElement).SetupGet(x => x.Attributes).Returns(new[] { attr });
            Mock.Get(attr).Setup(x => x.Matches(spec)).Returns(true);
            Mock.Get(metalSpecProvider).SetupGet(x => x.DefineMacro).Returns(spec);
            Mock.Get(annotationProvider)
                .Setup(x => x.GetAnnotation(context.CurrentElement, TagType.Start))
                .Returns(annotation);

            sut.ProcessContextAsync(context);

            Mock.Get(commenter)
                .Verify(x => x.AddCommentBefore(context.CurrentElement, annotation), Times.Once);
        }

        [Test, AutoMoqData]
        public void ProcessContextAsync_adds_annotation_after_define_slot_for_start_tag([Frozen] IGetsAnnotationForElement annotationProvider,
                                                                                        [Frozen] IAddsComment commenter,
                                                                                        [Frozen] IGetsMetalAttributeSpecs metalSpecProvider,
                                                                                        SourceAnnotationContextProcessor sut,
                                                                                        AttributeSpec spec,
                                                                                        IAttribute attr,
                                                                                        ExpressionContext context,
                                                                                        string annotation)
        {
            context.IsRootContext = false;
            Mock.Get(context.CurrentElement).SetupGet(x => x.IsImported).Returns(false);
            Mock.Get(context.CurrentElement).SetupGet(x => x.Attributes).Returns(new[] { attr });
            Mock.Get(attr).Setup(x => x.Matches(spec)).Returns(true);
            Mock.Get(metalSpecProvider).SetupGet(x => x.DefineSlot).Returns(spec);
            Mock.Get(annotationProvider)
                .Setup(x => x.GetAnnotation(context.CurrentElement, TagType.Start))
                .Returns(annotation);

            sut.ProcessContextAsync(context);

            Mock.Get(commenter)
                .Verify(x => x.AddCommentAfter(context.CurrentElement, annotation), Times.Once);
        }
    }
}
