using System;
using System.Collections.Generic;

namespace ZptSharp.Dom
{
    /// <summary>
    /// A stub node with no backing implementation, for testing purposes.
    /// Members are all intentionally virtual so that this class may be mocked.
    /// </summary>
    public class StubNode : NodeBase
    {
        public override IList<IAttribute> Attributes { get; } = new List<IAttribute>();

        public override IList<INode> ChildNodes { get; } = new List<INode>();

        public override bool IsElement => true;

        public override INode GetCopy() => this;

        public override bool IsInNamespace(Namespace @namespace) => false;

        public override INode CreateComment(string commentText) => throw new NotImplementedException();

        public override INode CreateTextNode(string content) => throw new NotImplementedException();

        public override IList<INode> ParseAsNodes(string markup) => throw new NotImplementedException();

        public override IAttribute CreateAttribute(AttributeSpec spec) => throw new NotImplementedException();

        public StubNode(IDocument document) : base(document) { }
    }
}
