using System;
using NUnit.Framework;

namespace ZptSharp.Rendering
{
    [TestFixture, Parallelizable]
    public class OtherSourceInfoTests
    {
        [Test, AutoMoqData]
        public void Ctor_throws_if_location_is_null()
        {
            Assert.That(() => new OtherSourceInfo(null), Throws.ArgumentNullException);
        }

        [Test, AutoMoqData]
        public void Ctor_does_not_throw_if_location_is_not_null(string location)
        {
            Assert.That(() => new OtherSourceInfo(location), Throws.Nothing);
        }

        [Test, AutoMoqData]
        public void ToString_returns_location(string location)
        {
            Assert.That(() => new OtherSourceInfo(location).ToString(), Is.EqualTo(location));
        }

        [Test, AutoMoqData]
        public void GetHashCode_returns_location_hash_code(string location)
        {
            Assert.That(() => new OtherSourceInfo(location).GetHashCode(), Is.EqualTo(location.GetHashCode()));
        }

        [Test, AutoMoqData]
        public void Equals_returns_false_for_object_of_other_type(OtherSourceInfo sut, object other)
        {
            Assert.That(() => sut.Equals(other), Is.False);
        }

        [Test, AutoMoqData]
        public void Equals_returns_false_for_other_type_of_source_info(OtherSourceInfo sut, IDocumentSourceInfo other)
        {
            Assert.That(() => sut.Equals(other), Is.False);
        }

        [Test, AutoMoqData]
        public void Equals_returns_false_for_null(OtherSourceInfo sut)
        {
            Assert.That(() => sut.Equals(null), Is.False);
        }

        [Test, AutoMoqData]
        public void Equals_returns_true_when_reference_equal(OtherSourceInfo sut)
        {
#pragma warning disable RECS0088 // Comparing equal expression for equality is usually useless
            Assert.That(() => sut.Equals(sut), Is.True);
#pragma warning restore RECS0088 // Comparing equal expression for equality is usually useless
        }

        [Test, AutoMoqData]
        public void Equals_returns_false_for_different_location(string location1, string location2)
        {
            var source1 = new OtherSourceInfo(location1);
            var source2 = new OtherSourceInfo(location2);
            Assert.That(() => source1.Equals(source2), Is.False);
        }

        [Test, AutoMoqData]
        public void Equals_returns_true_for_same_location(string location)
        {
            var source1 = new OtherSourceInfo(location);
            var source2 = new OtherSourceInfo(location);
            Assert.That(() => source1.Equals(source2), Is.True);
        }
    }
}
