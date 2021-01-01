using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using ZptSharp.Config;

namespace ZptSharp.Cli
{
    [TestFixture, Parallelizable]
    public class BulkRenderingRequestFactoryTests
    {
        [Test,AutoMoqData]
        public void GetRequestAsync_throws_ANE_if_args_are_null(BulkRenderingRequestFactory sut)
        {
            Assert.That(async () => await sut.GetRequestAsync(null), Throws.ArgumentNullException);
        }

        [Test,AutoMoqData]
        public void GetRequestAsync_throws_if_args_has_no_root_path(BulkRenderingRequestFactory sut, CliArguments args)
        {
            args.RootPath.Clear();
            Assert.That(async () => await sut.GetRequestAsync(args), Throws.ArgumentException);
        }

        [Test,AutoMoqData]
        public void GetRequestAsync_throws_if_args_has_more_than_one_root_path(BulkRenderingRequestFactory sut, CliArguments args)
        {
            args.RootPath = new [] { "Foo", "Bar" };
            Assert.That(async () => await sut.GetRequestAsync(args), Throws.ArgumentException);
        }

        [Test,AutoMoqData]
        public async Task GetRequestAsync_returns_wildcard_include_path_if_nothing_included(BulkRenderingRequestFactory sut, CliArguments args)
        {
            args.RootPath = new [] { "Foo" };
            args.CommaSeparatedIncludePatterns = null;

            var result = await sut.GetRequestAsync(args);

            Assert.That(result?.IncludedPaths, Is.EqualTo(new [] { "*.*" }));
        }

        [Test,AutoMoqData]
        public async Task GetRequestAsync_returns_parsed_include_paths_when_specified(BulkRenderingRequestFactory sut, CliArguments args)
        {
            args.RootPath = new [] { "Foo" };
            args.CommaSeparatedIncludePatterns = "Foo,Bar,Baz";

            var result = await sut.GetRequestAsync(args);

            Assert.That(result?.IncludedPaths, Is.EqualTo(new [] { "Foo", "Bar", "Baz" }));
        }

        [Test,AutoMoqData]
        public async Task GetRequestAsync_returns_current_directory_if_output_directory_is_empty(BulkRenderingRequestFactory sut, CliArguments args)
        {
            args.RootPath = new [] { "Foo" };
            args.OutputPath = null;

            var result = await sut.GetRequestAsync(args);

            Assert.That(result?.OutputPath, Is.EqualTo(Directory.GetCurrentDirectory()));
        }

        [Test,AutoMoqData]
        public async Task GetRequestAsync_returns_config_with_source_annotation_if_requested(BulkRenderingRequestFactory sut, CliArguments args)
        {
            args.RootPath = new [] { "Foo" };
            args.EnableSourceAnnotation = true;

            var result = await sut.GetRequestAsync(args);

            Assert.That(result.RenderingConfig,
                        Has.Property(nameof(RenderingConfig.IncludeSourceAnnotation)).True
                            .And.Property(nameof(RenderingConfig.SourceAnnotationBasePath)).EqualTo("Foo"));
        }

        [Test,AutoMoqData]
        public async Task GetRequestAsync_returns_config_without_source_annotation_if_not_requested(BulkRenderingRequestFactory sut, CliArguments args)
        {
            args.RootPath = new [] { "Foo" };
            args.EnableSourceAnnotation = false;

            var result = await sut.GetRequestAsync(args);

            Assert.That(result.RenderingConfig,
                        Has.Property(nameof(RenderingConfig.IncludeSourceAnnotation)).False
                            .And.Property(nameof(RenderingConfig.SourceAnnotationBasePath)).Null);
        }

        [Test,AutoMoqData]
        public async Task GetRequestAsync_returns_config_with_keyword_options_if_specified(BulkRenderingRequestFactory sut, CliArguments args)
        {
            args.RootPath = new [] { "Foo" };
            args.CommaSeparatedKeywordOptions = "weather=cloudy,time=13:45";

            var result = await sut.GetRequestAsync(args);

            Assert.That(result.RenderingConfig.KeywordOptions,
                        Is.EquivalentTo(new Dictionary<string,object> { { "weather", "cloudy" }, { "time", "13:45" } }));
        }
    }
}