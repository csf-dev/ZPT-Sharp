using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using ZptSharp.Util;

namespace ZptSharp.BulkRendering
{
    [TestFixture, Parallelizable]
    public class InputFilesProviderTests
    {
        char Slash = Path.DirectorySeparatorChar;

        [Test,AutoMoqData]
        public async Task GetInputFilesAsync_returns_correct_count_of_results_for_a_single_file_match(InputFilesProvider sut)
        {
            var request = GetBaseRequest();
            request.IncludedPaths.Add($"ADirectory{Slash}AnotherFile.txt");

            var result = await sut.GetInputFilesAsync(request);

            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [Test,AutoMoqData]
        public async Task GetInputFilesAsync_returns_correct_count_of_results_for_a_directory_search(InputFilesProvider sut)
        {
            var request = GetBaseRequest();
            request.IncludedPaths.Add($"ADirectory{Slash}*.txt");

            var result = await sut.GetInputFilesAsync(request);

            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test,AutoMoqData]
        public async Task GetInputFilesAsync_returns_correct_count_of_results_for_a_recursive_search(InputFilesProvider sut)
        {
            var request = GetBaseRequest();
            request.IncludedPaths.Add($"**{Slash}*.txt");

            var result = await sut.GetInputFilesAsync(request);

            Assert.That(result.Count(), Is.EqualTo(4));
        }

        [Test,AutoMoqData]
        public async Task GetInputFilesAsync_returns_correct_count_of_results_for_a_recursive_search_with_an_exclusion(InputFilesProvider sut)
        {
            var request = GetBaseRequest();
            request.IncludedPaths.Add($"**{Slash}*.txt");
            request.ExcludedPaths.Add($"**{Slash}Another*.*");

            var result = await sut.GetInputFilesAsync(request);

            Assert.That(result.Count(), Is.EqualTo(3));
        }

        [Test,AutoMoqData]
        public async Task GetInputFilesAsync_returns_correct_count_of_results_for_a_recursive_search_with_multiple_inputs(InputFilesProvider sut)
        {
            var request = GetBaseRequest();
            request.IncludedPaths.Add($"**{Slash}*.txt");
            request.IncludedPaths.Add($"**{Slash}*.xml");

            var result = await sut.GetInputFilesAsync(request);

            Assert.That(result.Count(), Is.EqualTo(6));
        }

        [Test,AutoMoqData]
        public async Task GetInputFilesAsync_returns_correct_absolute_and_relative_paths_for_a_single_file_match(InputFilesProvider sut)
        {
            var request = GetBaseRequest();
            request.IncludedPaths.Add($"ADirectory{Slash}AnotherFile.txt");

            var result = await sut.GetInputFilesAsync(request);

            Assert.That(result.Single(),
                        Has.Property(nameof(InputFile.AbsolutePath)).EqualTo(Path.Combine(RootPath, "ADirectory", "AnotherFile.txt"))
                            .And.Property(nameof(InputFile.RelativePath)).EqualTo(Path.Combine("ADirectory", "AnotherFile.txt")));
        }

        [Test,AutoMoqData]
        public async Task GetInputFilesAsync_returns_an_item_with_correct_absolute_and_relative_paths_for_a_recursive_search(InputFilesProvider sut)
        {
            var request = GetBaseRequest();
            request.IncludedPaths.Add($"**{Slash}*.txt");

            var result = await sut.GetInputFilesAsync(request);

            Assert.That(result,
                        Has.One.With.Property(nameof(InputFile.AbsolutePath)).EqualTo(Path.Combine(RootPath, "ADirectory", "SubDirectory", "SubFile.txt"))
                            .And.Property(nameof(InputFile.RelativePath)).EqualTo(Path.Combine("ADirectory", "SubDirectory", "SubFile.txt")));
        }

        string RootPath => TestFiles.GetPath(nameof(InputFilesProviderTests));

        BulkRenderingRequest GetBaseRequest()
            => new BulkRenderingRequest { InputRootPath = RootPath };
    }
}