using System;
using System.Linq;
using NUnit.Framework;

namespace ZptSharp.PathExpressions
{
    [TestFixture,Parallelizable]
    public class PathExpressionParserTests
    {
        [Test, AutoMoqData]
        public void Parse_can_parse_an_expression_which_has_a_single_path_part(PathExpressionParser sut)
        {
            var result = sut.Parse("foo");

            Assert.That(result?.Alternates?.Single().Parts?.Single(),
                        Is.InstanceOf<PathExpression.NamedPathPart>()
                            .With.Property(nameof(PathExpression.NamedPathPart.Name)).EqualTo("foo"));
        }

        [Test, AutoMoqData]
        public void Parse_can_parse_an_expression_which_has_a_three_part_path(PathExpressionParser sut)
        {
            var result = sut.Parse("foo/bar/baz");

            Assert.That(result?.Alternates?.Single().Parts?.Cast<PathExpression.NamedPathPart>().Select(x => x.Name),
                        Is.EqualTo(new[] { "foo", "bar", "baz" }));
        }

        [Test, AutoMoqData]
        public void Parse_can_parse_an_expression_which_has_two_alternate_expressions(PathExpressionParser sut)
        {
            var result = sut.Parse("foo/bar/baz | one/two | wibble/wobble");

            Assert.That(result?.Alternates[0].Parts?.Cast<PathExpression.NamedPathPart>().Select(x => x.Name),
                        Is.EqualTo(new[] { "foo", "bar", "baz" }),
                        "Correct parts for first alternate expression");
            Assert.That(result?.Alternates[1].Parts?.Cast<PathExpression.NamedPathPart>().Select(x => x.Name),
                        Is.EqualTo(new[] { "one", "two" }),
                        "Correct parts for second alternate expression");
            Assert.That(result?.Alternates[2].Parts?.Cast<PathExpression.NamedPathPart>().Select(x => x.Name),
                        Is.EqualTo(new[] { "wibble", "wobble" }),
                        "Correct parts for third alternate expression");
        }

        [Test, AutoMoqData]
        public void Parse_can_parse_an_expression_which_includes_an_interpolated_part(PathExpressionParser sut)
        {
            var result = sut.Parse("foo/?bar/baz");

            Assert.That(result?.Alternates?.Single().Parts[1], Is.InstanceOf<PathExpression.InterpolatedPathPart>());
        }

        [Test, AutoMoqData]
        public void Parse_can_parse_the_correct_interpolated_expression_for_an_interpolated_part(PathExpressionParser sut)
        {
            var result = sut.Parse("foo/?bar/baz");

            var interpolated = result?.Alternates?.Single().Parts[1] as PathExpression.InterpolatedPathPart;
            Assert.That(interpolated?.Expression?.Alternates?.Single().Parts?.Single(),
                        Is.InstanceOf<PathExpression.NamedPathPart>()
                            .With.Property(nameof(PathExpression.NamedPathPart.Name)).EqualTo("bar"));
        }

        [Test, AutoMoqData]
        public void Parse_throws_ANE_if_the_expression_is_null(PathExpressionParser sut)
        {
            Assert.That(() => sut.Parse(null), Throws.ArgumentNullException);
        }

        [Test, AutoMoqData]
        public void Parse_throws_if_the_expression_is_empty_string(PathExpressionParser sut)
        {
            Assert.That(() => sut.Parse(String.Empty), Throws.InstanceOf<CannotParsePathException>());
        }

        [Test, AutoMoqData]
        public void Parse_throws_if_an_alternate_expression_is_whitespace(PathExpressionParser sut)
        {
            Assert.That(() => sut.Parse("foo | "), Throws.InstanceOf<CannotParsePathException>());
        }

        [Test, AutoMoqData]
        public void Parse_throws_if_an_expression_is_whitespace(PathExpressionParser sut)
        {
            Assert.That(() => sut.Parse("   "), Throws.InstanceOf<CannotParsePathException>());
        }

        [Test, AutoMoqData]
        public void Parse_does_not_throw_if_a_path_part_is_whitespace_only(PathExpressionParser sut)
        {
            Assert.That(() => sut.Parse("foo/   /bar"), Throws.Nothing);
        }

        [Test, AutoMoqData]
        public void Parse_throws_if_a_path_part_is_missing(PathExpressionParser sut)
        {
            Assert.That(() => sut.Parse("foo//bar"), Throws.InstanceOf<CannotParsePathException>());
        }

        [Test, AutoMoqData]
        public void Parse_throws_if_first_path_part_is_not_a_variable_name(PathExpressionParser sut)
        {
            Assert.That(() => sut.Parse("../nope"), Throws.InstanceOf<CannotParsePathException>());
        }

        [Test, AutoMoqData]
        public void Parse_throws_if_subsequent_path_part_contains_illegal_character(PathExpressionParser sut)
        {
            Assert.That(() => sut.Parse("var/nope!"), Throws.InstanceOf<CannotParsePathException>());
        }

        [Test, AutoMoqData]
        public void Parse_does_not_throws_if_subsequent_path_parts_contain_permitted_punctuation(PathExpressionParser sut)
        {
            Assert.That(() => sut.Parse("var/../this-is-fine"), Throws.Nothing);
        }
    }
}
