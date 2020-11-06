using System;
using NUnit.Framework;

namespace ZptSharp.Expressions
{
    [TestFixture,Parallelizable]
    public class PrefixExpressionTypeProviderTests
    {
        [Test, AutoMoqData]
        public void GetExpressionType_returns_foobar_for_a_foobar_expression(PrefixExpressionTypeProvider sut)
        {
            Assert.That(() => sut.GetExpressionType("foobar:baz"), Is.EqualTo("foobar"));
        }

        [Test, AutoMoqData]
        public void GetExpressionType_returns_null_for_an_expression_with_no_prefix(PrefixExpressionTypeProvider sut)
        {
            Assert.That(() => sut.GetExpressionType("baz"), Is.Null);
        }

        [Test, AutoMoqData]
        public void GetExpressionType_returns_null_for_an_empty_string_expression(PrefixExpressionTypeProvider sut)
        {
            Assert.That(() => sut.GetExpressionType(String.Empty), Is.Null);
        }

        [Test, AutoMoqData]
        public void GetExpressionType_throws_ANE_for_a_null_expression(PrefixExpressionTypeProvider sut)
        {
            Assert.That(() => sut.GetExpressionType(null), Throws.ArgumentNullException);
        }
    }
}
