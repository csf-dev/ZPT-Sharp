using System;
using AutoFixture.NUnit3;
using NUnit.Framework;

namespace ZptSharp.Rendering
{
    [TestFixture,Parallelizable]
    public class NodeSourceInfoTests
    {
        [Test, AutoMoqData]
        public void Ctor_throws_ANE_if_document_source_is_null()
        {
            Assert.That(() => new NodeSourceInfo(null), Throws.ArgumentNullException);
        }

        [Test, AutoMoqData]
        public void Equals_returns_true_if_instances_are_reference_equal(NodeSourceInfo sut)
        {
#pragma warning disable RECS0088 // Comparing equal expression for equality is usually useless
            Assert.That(() => sut.Equals(sut), Is.True);
#pragma warning restore RECS0088 // Comparing equal expression for equality is usually useless
        }

        [Test, AutoMoqData]
        public void Equals_returns_true_if_two_instances_have_same_document_and_line_numbers(IDocumentSourceInfo documentSource,
                                                                                            int startLineNumber,
                                                                                            int endLineNumber)
        {
            var first = new NodeSourceInfo(documentSource, startLineNumber, endLineNumber);
            var second = new NodeSourceInfo(documentSource, startLineNumber, endLineNumber);
            Assert.That(() => first.Equals(second), Is.True);
        }

        [Test, AutoMoqData]
        public void Equals_returns_false_if_two_instances_have_different_start_line_numbers(IDocumentSourceInfo documentSource,
                                                                                            int endLineNumber)
        {
            var first = new NodeSourceInfo(documentSource, 5, endLineNumber);
            var second = new NodeSourceInfo(documentSource, 6, endLineNumber);
            Assert.That(() => first.Equals(second), Is.False);
        }

        [Test, AutoMoqData]
        public void Equals_returns_false_if_two_instances_have_different_end_line_numbers(IDocumentSourceInfo documentSource,
                                                                                          int startLineNumber)
        {
            var first = new NodeSourceInfo(documentSource, startLineNumber, 5);
            var second = new NodeSourceInfo(documentSource, startLineNumber, 10);
            Assert.That(() => first.Equals(second), Is.False);
        }

        [Test, AutoMoqData]
        public void Equals_returns_false_if_start_line_number_is_null(IDocumentSourceInfo documentSource)
        {
            var first = new NodeSourceInfo(documentSource, null, 10);
            var second = new NodeSourceInfo(documentSource, 6, 10);
            Assert.That(() => first.Equals(second), Is.False);
        }

        [Test, AutoMoqData]
        public void Equals_returns_false_if_other_start_line_number_is_null(IDocumentSourceInfo documentSource)
        {
            var first = new NodeSourceInfo(documentSource, 5, 10);
            var second = new NodeSourceInfo(documentSource, null, 10);
            Assert.That(() => first.Equals(second), Is.False);
        }

        [Test, AutoMoqData]
        public void Equals_returns_false_if_both_line_numbers_are_null(IDocumentSourceInfo documentSource)
        {
            var first = new NodeSourceInfo(documentSource);
            var second = new NodeSourceInfo(documentSource);
            Assert.That(() => first.Equals(second), Is.False);
        }

        [Test, AutoMoqData]
        public void Equals_returns_false_if_documents_differ(IDocumentSourceInfo documentSource1,
                                                             IDocumentSourceInfo documentSource2,
                                                             int lineNumber)
        {
            var first = new NodeSourceInfo(documentSource1, lineNumber);
            var second = new NodeSourceInfo(documentSource2, lineNumber);
            Assert.That(() => first.Equals(second), Is.False);
        }

        [Test, AutoMoqData]
        public void Equals_returns_false_comparing_with_null(NodeSourceInfo sut)
        {
            Assert.That(() => sut.Equals(null), Is.False);
        }

        [Test, AutoMoqData]
        public void GetHashCode_with_same_document_and_line_number_is_equal(IDocumentSourceInfo documentSource,
                                                                            int startLineNumber,
                                                                            int endLineNumber)
        {
            var first = new NodeSourceInfo(documentSource, startLineNumber, endLineNumber);
            var second = new NodeSourceInfo(documentSource, startLineNumber, endLineNumber);
            Assert.That(() => first.GetHashCode(), Is.EqualTo(second.GetHashCode()));
        }

        [Test, AutoMoqData]
        public void GetHashCode_returns_different_result_for_different_start_line_numbers(IDocumentSourceInfo documentSource)
        {
            var first = new NodeSourceInfo(documentSource, 5, 10);
            var second = new NodeSourceInfo(documentSource, 6, 10);
            Assert.That(() => first.GetHashCode(), Is.Not.EqualTo(second.GetHashCode()));
        }

        [Test, AutoMoqData]
        public void GetHashCode_returns_different_result_for_different_end_line_numbers(IDocumentSourceInfo documentSource)
        {
            var first = new NodeSourceInfo(documentSource, 10, 5);
            var second = new NodeSourceInfo(documentSource, 10, 6);
            Assert.That(() => first.GetHashCode(), Is.Not.EqualTo(second.GetHashCode()));
        }

        [Test, AutoMoqData]
        public void GetHashCode_returns_different_result_for_two_instances_without_line_numbers(IDocumentSourceInfo documentSource)
        {
            var first = new NodeSourceInfo(documentSource);
            var second = new NodeSourceInfo(documentSource);
            Assert.That(() => first.GetHashCode(), Is.Not.EqualTo(second.GetHashCode()));
        }
    }
}
