using System;
using System.Collections.Generic;

namespace ZptSharp.Dom
{
    /// <summary>
    /// A stub element with no backing implementation, for testing purposes.
    /// Members are all intentionally virtual so that this class may be mocked.
    /// </summary>
    public class StubElement : ElementBase
    {
        public override IList<IAttribute> Attributes { get; } = new List<IAttribute>();

        public override IList<IElement> ChildElements { get; } = new List<IElement>();

        public override void ReplaceWith(IElement replacement) => throw new NotImplementedException();

        public StubElement(IDocument document) : base(document) { }
    }
}
