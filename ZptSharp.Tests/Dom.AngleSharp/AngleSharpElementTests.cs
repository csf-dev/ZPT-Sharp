using System;
using NUnit.Framework;
using ZptSharp.Util;

namespace ZptSharp.Dom
{
    [TestFixture,Parallelizable]
    public class AngleSharpElementTests
    {
        [Test, AutoMoqData]
        public void Ctor_creates_the_appropriate_object_model()
        {
            var testFilePath = TestFiles.GetPath("SampleZptDocument.txt");
        }
    }
}
