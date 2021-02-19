using System;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;

namespace ZptSharp.Expressions
{
    [TestFixture,Parallelizable]
    public class RepetitionInfoTests
    {
        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_correct_result_for_index(RepetitionInfo sut)
        {
            sut.CurrentIndex = 5;
            var result = await sut.TryGetValueAsync("index");

            Assert.That(result, Has.Property(nameof(GetValueResult.Success)).True
                                .And.Property(nameof(GetValueResult.Value)).EqualTo(5));
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_correct_result_for_number(RepetitionInfo sut)
        {
            sut.CurrentIndex = 5;
            var result = await sut.TryGetValueAsync("number");

            Assert.That(result, Has.Property(nameof(GetValueResult.Success)).True
                                .And.Property(nameof(GetValueResult.Value)).EqualTo(6));
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_true_for_even_when_index_is_4(RepetitionInfo sut)
        {
            sut.CurrentIndex = 4;
            var result = await sut.TryGetValueAsync("even");

            Assert.That(result, Has.Property(nameof(GetValueResult.Success)).True
                                .And.Property(nameof(GetValueResult.Value)).True);
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_false_for_even_when_index_is_5(RepetitionInfo sut)
        {
            sut.CurrentIndex = 5;
            var result = await sut.TryGetValueAsync("even");

            Assert.That(result, Has.Property(nameof(GetValueResult.Success)).True
                                .And.Property(nameof(GetValueResult.Value)).False);
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_true_for_odd_when_index_is_5(RepetitionInfo sut)
        {
            sut.CurrentIndex = 5;
            var result = await sut.TryGetValueAsync("odd");

            Assert.That(result, Has.Property(nameof(GetValueResult.Success)).True
                                .And.Property(nameof(GetValueResult.Value)).True);
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_false_for_odd_when_index_is_4(RepetitionInfo sut)
        {
            sut.CurrentIndex = 4;
            var result = await sut.TryGetValueAsync("odd");

            Assert.That(result, Has.Property(nameof(GetValueResult.Success)).True
                                .And.Property(nameof(GetValueResult.Value)).False);
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_true_for_start_when_index_is_zero(RepetitionInfo sut)
        {
            sut.CurrentIndex = 0;
            var result = await sut.TryGetValueAsync("start");

            Assert.That(result, Has.Property(nameof(GetValueResult.Success)).True
                                .And.Property(nameof(GetValueResult.Value)).True);
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_false_for_start_when_index_is_one(RepetitionInfo sut)
        {
            sut.CurrentIndex = 1;
            var result = await sut.TryGetValueAsync("start");

            Assert.That(result, Has.Property(nameof(GetValueResult.Success)).True
                                .And.Property(nameof(GetValueResult.Value)).False);
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_true_for_end_when_index_is_count_minus_one(RepetitionInfo sut)
        {
            sut.CurrentIndex = 4;
            sut.Count = 5;
            var result = await sut.TryGetValueAsync("end");

            Assert.That(result, Has.Property(nameof(GetValueResult.Success)).True
                                .And.Property(nameof(GetValueResult.Value)).True);
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_false_for_end_when_index_is_less_than_count_minus_one(RepetitionInfo sut)
        {
            sut.CurrentIndex = 3;
            sut.Count = 5;
            var result = await sut.TryGetValueAsync("end");

            Assert.That(result, Has.Property(nameof(GetValueResult.Success)).True
                                .And.Property(nameof(GetValueResult.Value)).False);
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_correct_result_for_length(RepetitionInfo sut)
        {
            sut.Count = 9;
            var result = await sut.TryGetValueAsync("length");

            Assert.That(result, Has.Property(nameof(GetValueResult.Success)).True
                                .And.Property(nameof(GetValueResult.Value)).EqualTo(9));
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_result_from_service_for_letter([Frozen] IGetsAlphabeticValueForNumber alpha,
                                                                                  RepetitionInfo sut,
                                                                                  int index)
        {
            Mock.Get(alpha)
                .Setup(x => x.GetAlphabeticValue(index))
                .Returns("abcdef");
            sut.CurrentIndex = index;
            var result = await sut.TryGetValueAsync("letter");

            Assert.That(result, Has.Property(nameof(GetValueResult.Success)).True
                                .And.Property(nameof(GetValueResult.Value)).EqualTo("abcdef"));
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_uppercase_result_from_service_for_Letter([Frozen] IGetsAlphabeticValueForNumber alpha,
                                                                                            RepetitionInfo sut,
                                                                                            int index)
        {
            Mock.Get(alpha)
                .Setup(x => x.GetAlphabeticValue(index))
                .Returns("abcdef");
            sut.CurrentIndex = index;
            var result = await sut.TryGetValueAsync("Letter");

            Assert.That(result, Has.Property(nameof(GetValueResult.Success)).True
                                .And.Property(nameof(GetValueResult.Value)).EqualTo("ABCDEF"));
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_lowercase_result_from_service_for_roman([Frozen] IGetsRomanNumeralForNumber roman,
                                                                                           RepetitionInfo sut,
                                                                                           int index)
        {
            Mock.Get(roman)
                .Setup(x => x.GetRomanNumeral(index + 1))
                .Returns("VVII");
            sut.CurrentIndex = index;
            var result = await sut.TryGetValueAsync("roman");

            Assert.That(result, Has.Property(nameof(GetValueResult.Success)).True
                                .And.Property(nameof(GetValueResult.Value)).EqualTo("vvii"));
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_result_from_service_for_Roman([Frozen] IGetsRomanNumeralForNumber roman,
                                                                                 RepetitionInfo sut,
                                                                                 int index)
        {
            Mock.Get(roman)
                .Setup(x => x.GetRomanNumeral(index + 1))
                .Returns("VVII");
            sut.CurrentIndex = index;
            var result = await sut.TryGetValueAsync("Roman");

            Assert.That(result, Has.Property(nameof(GetValueResult.Success)).True
                                .And.Property(nameof(GetValueResult.Value)).EqualTo("VVII"));
        }
    }
}
