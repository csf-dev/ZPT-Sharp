using System;
using System.IO;
using NUnit.Framework;
using ZptSharp.Util;

namespace ZptSharp.Rendering
{
    [TestFixture,Parallelizable]
    public class FileSourceInfoTests
    {
        [Test, AutoMoqData]
        public void Ctor_throws_if_path_is_null()
        {
            Assert.That(() => new FileSourceInfo(null), Throws.ArgumentNullException);
        }

        [Test, AutoMoqData]
        public void Ctor_does_not_throw_if_path_is_not_null(string path)
        {
            Assert.That(() => new FileSourceInfo(path), Throws.Nothing);
        }

        [Test, AutoMoqData]
        public void ToString_returns_path(string path)
        {
            Assert.That(() => new FileSourceInfo(path).ToString(), Is.EqualTo(path));
        }

        [Test, AutoMoqData]
        public void GetHashCode_returns_path_hash_code(string path)
        {
            Assert.That(() => new FileSourceInfo(path).GetHashCode(), Is.EqualTo(path.GetHashCode()));
        }

        [Test, AutoMoqData]
        public void Equals_returns_false_for_object_of_other_type(FileSourceInfo sut, object other)
        {
            Assert.That(() => sut.Equals(other), Is.False);
        }

        [Test, AutoMoqData]
        public void Equals_returns_false_for_other_type_of_source_info(FileSourceInfo sut, IDocumentSourceInfo other)
        {
            Assert.That(() => sut.Equals(other), Is.False);
        }

        [Test, AutoMoqData]
        public void Equals_returns_false_for_null(FileSourceInfo sut)
        {
            Assert.That(() => sut.Equals(null), Is.False);
        }

        [Test, AutoMoqData]
        public void Equals_returns_true_when_reference_equal(FileSourceInfo sut)
        {
#pragma warning disable RECS0088 // Comparing equal expression for equality is usually useless
            Assert.That(() => sut.Equals(sut), Is.True);
#pragma warning restore RECS0088 // Comparing equal expression for equality is usually useless
        }

        [Test, AutoMoqData]
        public void Equals_returns_false_for_different_path(string path1, string path2)
        {
            var source1 = new FileSourceInfo(path1);
            var source2 = new FileSourceInfo(path2);
            Assert.That(() => source1.Equals(source2), Is.False);
        }

        [Test, AutoMoqData]
        public void Equals_returns_true_for_same_path(string path)
        {
            var source1 = new FileSourceInfo(path);
            var source2 = new FileSourceInfo(path);
            Assert.That(() => source1.Equals(source2), Is.True);
        }

        [Test, AutoMoqData]
        public void GetContainer_returns_a_template_directory_instance_for_parent_directory()
        {
            var path = Path.Combine(TestFiles.GetPath("ZptIntegrationTests"), "SourceDocuments", "acme_template.html");
            var expectedPath = Path.Combine(TestFiles.GetPath("ZptIntegrationTests"), "SourceDocuments");
            var sut = new FileSourceInfo(path);

            Assert.That(() => sut.GetContainer(), Is.InstanceOf<TemplateDirectory>()
                                                  .And.Property(nameof(TemplateDirectory.Path)).EqualTo(expectedPath));
        }
    }
}
