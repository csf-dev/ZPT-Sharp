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

        public override string Value { get; }

        public override bool Matches(AttributeSpec spec) => String.Equals(spec?.Name, Name);

        public override bool IsInNamespace(Namespace @namespace) => false;

        public StubAttribute(IElement element, string name, string value) : base(element)
        {
            // It's OK to suppress these here, this class is not for prod usage

#pragma warning disable RECS0021 // Warns about calls to virtual member functions occuring in the constructor
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Value = value ?? throw new ArgumentNullException(nameof(value));
#pragma warning restore RECS0021 // Warns about calls to virtual member functions occuring in the constructor
        }
    }
}
