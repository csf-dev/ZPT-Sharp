using System;
namespace ZptSharp.Dom
{
    /// <summary>
    /// A stub document with no backing implementation, for testing purposes.
    /// Members are all intentionally virtual so that this class may be mocked.
    /// </summary>
    public class StubDocument : DocumentBase
    {
        public virtual IElement Root { get; set; }

        public override IElement RootElement => Root;

        public StubDocument(Rendering.IDocumentSourceInfo source) : base(source) { }
    }
}
