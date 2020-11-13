using System;
namespace ZptSharp.Dom
{
    /// <summary>
    /// A stub document with no backing implementation, for testing purposes.
    /// Members are all intentionally virtual so that this class may be mocked.
    /// </summary>
    public class StubDocument : DocumentBase
    {
        public virtual INode Root { get; set; }

        public override INode RootElement => Root;

        public StubDocument(Rendering.IDocumentSourceInfo source) : base(source) { }
    }
}
