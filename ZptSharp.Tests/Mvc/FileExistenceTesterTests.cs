using NUnit.Framework;
using ZptSharp.Util;

namespace ZptSharp.Mvc
{
    [TestFixture, Parallelizable]
    public class FileExistenceTesterTests
    {
        [Test,AutoMoqData]
        public void DoesFileExist_returns_true_when_the_file_exists(FileExistenceTester sut)
        {
            Assert.That(() => sut.DoesFileExist(TestFiles.GetPath("SampleZptDocument.txt")), Is.True);
        }

        [Test,AutoMoqData]
        public void DoesFileExist_returns_false_when_the_file_does_not_exist(FileExistenceTester sut)
        {
            Assert.That(() => sut.DoesFileExist(TestFiles.GetPath("AFileWhichDoesNotExist.txt")), Is.False);
        }
    }
}