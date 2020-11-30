using System;
namespace ZptSharp.Dom
{
    /// <summary>
    /// A stub attribute with no backing implementation, for testing purposes.
    /// Members are all intentionally virtual so that this class may be mocked.
    /// </summary>
    public class StubAttribute : AttributeBase
    {
        public override string Name { get; }

        public override string Value { get; set; }

        public override bool Matches(AttributeSpec spec) => String.Equals(spec?.Name, Name);

        public override bool IsInNamespace(Namespace @namespace) => false;

        public override string ToString() => $"{Name}=\"{Value}\"";

        public override bool IsNamespaceDeclarationFor(Namespace @namespace) => false;

        public StubAttribute(string name)
        {
            // It's OK to suppress these here, this class is not for prod usage
#pragma warning disable RECS0021 // Warns about calls to virtual member functions occuring in the constructor
            Name = name ?? throw new ArgumentNullException(nameof(name));
#pragma warning restore RECS0021 // Warns about calls to virtual member functions occuring in the constructor
        }
    }
}
