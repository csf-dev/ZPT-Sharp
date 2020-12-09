using System;
using System.IO;
using AutoFixture.NUnit3;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using ZptSharp.Autofixture;
using ZptSharp.Config;
using ZptSharp.Rendering;

namespace ZptSharp.SourceAnnotation
{
    [TestFixture,Parallelizable]
    public class SourceAnnotationStringProviderTests
    {
        [Test, AutoMoqData]
        public void GetSourceInfo_returns_null_if_input_is_null([MockLogger,Frozen] ILogger<SourceAnnotationStringProvider> logger,
                                                                SourceAnnotationStringProvider sut)
        {
            Assert.That(() => sut.GetSourceInfo(null), Is.Null);
        }

        [Test, AutoMoqData]
        public void GetSourceInfo_returns_input_ToString_if_it_is_not_file_source_info([MockLogger,Frozen] ILogger<SourceAnnotationStringProvider> logger,
                                                                                       SourceAnnotationStringProvider sut,
                                                                                       string location)
        {
            var source = new OtherSourceInfo(location);
            Assert.That(() => sut.GetSourceInfo(source), Is.EqualTo(location));
        }

        [Test, AutoMoqData]
        public void GetSourceInfo_returns_input_ToString_if_it_is_file_source_info_but_config_source_root_is_null([MockLogger,Frozen] ILogger<SourceAnnotationStringProvider> logger,
                                                                                                                  [MockedConfig, Frozen] RenderingConfig config,
                                                                                                                  SourceAnnotationStringProvider sut)
        {
            Mock.Get(config).SetupGet(x => x.SourceAnnotationBasePath).Returns(() => null);
            var source = new FileSourceInfo(@"c:\foo\bar");
            Assert.That(() => sut.GetSourceInfo(source), Is.EqualTo(@"c:\foo\bar"));
        }

        [Test, AutoMoqData]
        public void GetSourceInfo_returns_input_ToString_if_config_source_root_is_not_base_of_file_path([MockLogger,Frozen] ILogger<SourceAnnotationStringProvider> logger,
                                                                                                        [MockedConfig, Frozen] RenderingConfig config,
                                                                                                        SourceAnnotationStringProvider sut)
        {
            Mock.Get(config).SetupGet(x => x.SourceAnnotationBasePath).Returns(@"c:\directory");
            var source = new FileSourceInfo(@"c:\foo\bar");
            Assert.That(() => sut.GetSourceInfo(source), Is.EqualTo(@"c:\foo\bar"));
        }

        [Test, AutoMoqData]
        public void GetSourceInfo_returns_file_path_relative_to_config_source_root_if_it_is_a_child([MockLogger,Frozen] ILogger<SourceAnnotationStringProvider> logger,
                                                                                                    [MockedConfig, Frozen] RenderingConfig config,
                                                                                                    SourceAnnotationStringProvider sut)
        {
            Mock.Get(config).SetupGet(x => x.SourceAnnotationBasePath).Returns($@"c:{Path.DirectorySeparatorChar}directory{Path.DirectorySeparatorChar}");
            var source = new FileSourceInfo($@"c:{Path.DirectorySeparatorChar}directory{Path.DirectorySeparatorChar}bar");
            Assert.That(() => sut.GetSourceInfo(source), Is.EqualTo(@"bar"));
        }

        [Test, AutoMoqData]
        public void GetSourceInfo_removes_leading_directory_separator_if_present([MockLogger,Frozen] ILogger<SourceAnnotationStringProvider> logger,
                                                                                 [MockedConfig, Frozen] RenderingConfig config,
                                                                                 SourceAnnotationStringProvider sut)
        {
            Mock.Get(config).SetupGet(x => x.SourceAnnotationBasePath).Returns($@"c:{Path.DirectorySeparatorChar}directory");
            var source = new FileSourceInfo($@"c:{Path.DirectorySeparatorChar}directory{Path.DirectorySeparatorChar}bar");
            Assert.That(() => sut.GetSourceInfo(source), Is.EqualTo(@"bar"));
        }

        [Test, AutoMoqData]
        public void GetStartTagInfo_appends_start_tag_line_if_it_is_not_null([MockLogger,Frozen] ILogger<SourceAnnotationStringProvider> logger,
                                                                             SourceAnnotationStringProvider sut, string location, int line)
        {
            var doc = new OtherSourceInfo(location);
            var source = new ElementSourceInfo(doc, line, null);
            Assert.That(() => sut.GetStartTagInfo(source), Is.EqualTo($"{location} (line {line})"));
        }

        [Test, AutoMoqData]
        public void GetStartTagInfo_appends_nothing_if_start_tag_line_is_null([MockLogger,Frozen] ILogger<SourceAnnotationStringProvider> logger,
                                                                              SourceAnnotationStringProvider sut, string location)
        {
            var doc = new OtherSourceInfo(location);
            var source = new ElementSourceInfo(doc);
            Assert.That(() => sut.GetStartTagInfo(source), Is.EqualTo(location));
        }

        [Test, AutoMoqData]
        public void GetEndTagInfo_appends_end_tag_line_if_it_is_not_null([MockLogger,Frozen] ILogger<SourceAnnotationStringProvider> logger,
                                                                         SourceAnnotationStringProvider sut, string location, int line)
        {
            var doc = new OtherSourceInfo(location);
            var source = new ElementSourceInfo(doc, null, line);
            Assert.That(() => sut.GetEndTagInfo(source), Is.EqualTo($"{location} (line {line})"));
        }

        [Test, AutoMoqData]
        public void GetEndTagInfo_appends_nothing_if_end_tag_line_is_null([MockLogger,Frozen] ILogger<SourceAnnotationStringProvider> logger,
                                                                          SourceAnnotationStringProvider sut, string location)
        {
            var doc = new OtherSourceInfo(location);
            var source = new ElementSourceInfo(doc);
            Assert.That(() => sut.GetEndTagInfo(source), Is.EqualTo(location));
        }
    }
}
