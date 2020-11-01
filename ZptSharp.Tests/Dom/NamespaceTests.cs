using System;
using NUnit.Framework;

namespace ZptSharp.Dom
{
    [TestFixture,Parallelizable]
    public class NamespaceTests
    {
        #region Equals

        [Test, AutoMoqData]
        public void Equals_returns_true_for_itself(string uri, string prefix)
        {
            var namespace1 = new Namespace(prefix, uri);

#pragma warning disable RECS0088 // Comparing equal expression for equality is usually useless
            Assert.That(() => namespace1.Equals(namespace1), Is.True);
#pragma warning restore RECS0088 // Comparing equal expression for equality is usually useless
        }

        [Test, AutoMoqData]
        public void Equals_returns_false_for_null(string uri, string prefix)
        {
            var namespace1 = new Namespace(prefix, uri);

            Assert.That(() => namespace1.Equals(null), Is.False);
        }

        [Test, AutoMoqData]
        public void Equals_returns_true_for_a_namespace_with_the_same_uri_regardless_of_prefix(string uri)
        {
            var namespace1 = new Namespace("Foo", uri);
            var namespace2 = new Namespace("Bar", uri);

            Assert.That(() => namespace1.Equals(namespace2), Is.True);
        }

        [Test, AutoMoqData]
        public void Equals_returns_false_for_a_namespace_with_a_different_uri()
        {
            var namespace1 = new Namespace("Foo", "A URI");
            var namespace2 = new Namespace("Bar", "Different URI");

            Assert.That(() => namespace1.Equals(namespace2), Is.False);
        }

        [Test, AutoMoqData]
        public void Equals_returns_false_for_a_namespace_with_a_different_uri_even_when_prefixes_are_the_same(string prefix)
        {
            var namespace1 = new Namespace(prefix, "A URI");
            var namespace2 = new Namespace(prefix, "Different URI");

            Assert.That(() => namespace1.Equals(namespace2), Is.False);
        }

        [Test, AutoMoqData]
        public void Equals_returns_false_for_a_namespace_with_a_null_uri_if_the_first_namespace_uri_is_not_null(string uri, string prefix)
        {
            var namespace1 = new Namespace(prefix, uri);
            var namespace2 = new Namespace(prefix, null);

            Assert.That(() => namespace1.Equals(namespace2), Is.False);
        }

        [Test, AutoMoqData]
        public void Equals_returns_false_for_a_namespace_with_a_uri_if_the_first_namespace_uri_is_null(string uri, string prefix)
        {
            var namespace1 = new Namespace(prefix);
            var namespace2 = new Namespace(prefix, uri);

            Assert.That(() => namespace1.Equals(namespace2), Is.False);
        }

        [Test, AutoMoqData]
        public void Equals_returns_true_for_a_namespace_with_the_same_prefix_if_uris_are_both_null(string prefix)
        {
            var namespace1 = new Namespace(prefix);
            var namespace2 = new Namespace(prefix);

            Assert.That(() => namespace1.Equals(namespace2), Is.True);
        }

        [Test, AutoMoqData]
        public void Equals_returns_false_for_a_namespace_with_different_prefixes_if_uris_are_both_null()
        {
            var namespace1 = new Namespace("Foo");
            var namespace2 = new Namespace("Bar");

            Assert.That(() => namespace1.Equals(namespace2), Is.False);
        }

        [Test, AutoMoqData]
        public void Equals_returns_true_for_a_namespace_with_null_prefixes_if_uris_are_both_null()
        {
            var namespace1 = new Namespace();
            var namespace2 = new Namespace();

            Assert.That(() => namespace1.Equals(namespace2), Is.True);
        }

        [Test, AutoMoqData]
        public void Equals_returns_false_for_an_unrelated_object(object other)
        {
            var namespace1 = new Namespace();

            Assert.That(() => namespace1.Equals(other), Is.False);
        }

        #endregion

        #region GetHashCode

        [Test, AutoMoqData]
        public void GetHashCode_returns_hash_code_of_uri_if_it_is_set(string uri)
        {
            var namespace1 = new Namespace(uri: uri);
            var expected = uri.GetHashCode();

            Assert.That(() => namespace1.GetHashCode(), Is.EqualTo(expected));
        }

        [Test, AutoMoqData]
        public void GetHashCode_returns_hash_code_of_uri_if_both_uri_and_prefix_are_set(string uri, string prefix)
        {
            var namespace1 = new Namespace(prefix, uri);
            var expected = uri.GetHashCode();

            Assert.That(() => namespace1.GetHashCode(), Is.EqualTo(expected));
        }

        [Test, AutoMoqData]
        public void GetHashCode_returns_hash_code_of_prefix_if_uri_is_null_but_prefix_is_set(string prefix)
        {
            var namespace1 = new Namespace(prefix);
            var expected = prefix.GetHashCode();

            Assert.That(() => namespace1.GetHashCode(), Is.EqualTo(expected));
        }

        [Test, AutoMoqData]
        public void GetHashCode_returns_zero_if_both_prefix_and_uri_are_null()
        {
            var namespace1 = new Namespace();
            Assert.That(() => namespace1.GetHashCode(), Is.EqualTo(0));
        }

        #endregion

        #region ToString

        [Test, AutoMoqData]
        public void ToString_returns_uri_if_it_is_set(string uri)
        {
            var namespace1 = new Namespace(uri: uri);
            Assert.That(() => namespace1.ToString(), Is.EqualTo(uri));
        }

        [Test, AutoMoqData]
        public void ToString_returns_prefix_and_uri_if_both_are_set()
        {
            var namespace1 = new Namespace("Foo", "Bar");
            Assert.That(() => namespace1.ToString(), Is.EqualTo("Foo:Bar"));
        }

        [Test, AutoMoqData]
        public void ToString_returns_prefix_if_uri_is_null_but_prefix_is_set(string prefix)
        {
            var namespace1 = new Namespace(prefix);
            Assert.That(() => namespace1.ToString(), Is.EqualTo(prefix));
        }

        [Test, AutoMoqData]
        public void ToString_returns_empty_string_if_both_prefix_and_uri_are_null()
        {
            var namespace1 = new Namespace();
            Assert.That(() => namespace1.ToString(), Is.Empty);
        }

        #endregion

        #region operators

        [Test, AutoMoqData]
        public void Operator_equals_returns_true_if_namespaces_are_equal(string uri)
        {
            var namespace1 = new Namespace(uri: uri);
            var namespace2 = new Namespace(uri: uri);

            Assert.That(() => namespace1 == namespace2, Is.True);
        }

        [Test, AutoMoqData]
        public void Operator_equals_returns_false_if_namespaces_are_not_equal()
        {
            var namespace1 = new Namespace(uri: "Foo");
            var namespace2 = new Namespace(uri: "Bar");

            Assert.That(() => namespace1 == namespace2, Is.False);
        }

        [Test, AutoMoqData]
        public void Operator_notequals_returns_false_if_namespaces_are_equal(string uri)
        {
            var namespace1 = new Namespace(uri: uri);
            var namespace2 = new Namespace(uri: uri);

            Assert.That(() => namespace1 != namespace2, Is.False);
        }

        [Test, AutoMoqData]
        public void Operator_notequals_returns_true_if_namespaces_are_not_equal()
        {
            var namespace1 = new Namespace(uri: "Foo");
            var namespace2 = new Namespace(uri: "Bar");

            Assert.That(() => namespace1 != namespace2, Is.True);
        }

        #endregion

    }
}
