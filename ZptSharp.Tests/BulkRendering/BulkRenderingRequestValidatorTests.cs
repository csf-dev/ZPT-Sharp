using System;
using NUnit.Framework;

namespace ZptSharp.BulkRendering
{
    [TestFixture, Parallelizable]
    public class BulkRenderingRequestValidatorTests
    {
        [Test,AutoMoqData]
        public void AssertIsValid_throws_ane_if_request_is_null(BulkRenderingRequestValidator sut)
        {
            Assert.That(() => sut.AssertIsValid(null), Throws.ArgumentNullException);
        }

        [Test,AutoMoqData]
        public void AssertIsValid_throws_BulkRenderingException_if_request_has_no_input_paths(BulkRenderingRequestValidator sut, BulkRenderingRequest request)
        {
            request.IncludedPaths.Clear();
            Assert.That(() => sut.AssertIsValid(request), Throws.InstanceOf<BulkRenderingException>());
        }

        [Test,AutoMoqData]
        public void AssertIsValid_throws_BulkRenderingException_if_request_has_null_input_paths(BulkRenderingRequestValidator sut, BulkRenderingRequest request)
        {
            request.IncludedPaths = null;
            Assert.That(() => sut.AssertIsValid(request), Throws.InstanceOf<BulkRenderingException>());
        }

        [Test,AutoMoqData]
        public void AssertIsValid_throws_BulkRenderingException_if_request_has_no_output_path(BulkRenderingRequestValidator sut, BulkRenderingRequest request)
        {
            request.OutputPath = null;
            Assert.That(() => sut.AssertIsValid(request), Throws.InstanceOf<BulkRenderingException>());
        }

        [Test,AutoMoqData]
        public void AssertIsValid_throws_BulkRenderingException_if_request_has_empty_output_path(BulkRenderingRequestValidator sut, BulkRenderingRequest request)
        {
            request.OutputPath = String.Empty;
            Assert.That(() => sut.AssertIsValid(request), Throws.InstanceOf<BulkRenderingException>());
        }

        [Test,AutoMoqData]
        public void AssertIsValid_throws_nothing_for_valid_config(BulkRenderingRequestValidator sut, BulkRenderingRequest request)
        {
            Assert.That(() => sut.AssertIsValid(request), Throws.Nothing);
        }

        [Test,AutoMoqData]
        public void AssertIsValid_uses_current_directory_for_input_root_if_it_is_null(BulkRenderingRequestValidator sut, BulkRenderingRequest request)
        {
            request.InputRootPath = null;
            sut.AssertIsValid(request);
            Assert.That(request.InputRootPath, Is.EqualTo(Environment.CurrentDirectory));
        }
    }
}