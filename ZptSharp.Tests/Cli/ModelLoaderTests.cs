using System.Threading.Tasks;
using NUnit.Framework;
using ZptSharp.Util;

namespace ZptSharp.Cli
{
    [TestFixture, Parallelizable]
    public class ModelLoaderTests
    {
        [Test,AutoMoqData]
        public async Task LoadModelAsync_returns_dynamic_object_with_correct_properties(ModelLoader sut)
        {
            var filePath = TestFiles.GetPath(nameof(ModelLoaderTests), "sampleModel.json");
            dynamic model = await sut.LoadModelAsync(filePath);
            Assert.That(model.objectProp.items[1].name.ToString(), Is.EqualTo("Jane Doe"));
        }
    }
}