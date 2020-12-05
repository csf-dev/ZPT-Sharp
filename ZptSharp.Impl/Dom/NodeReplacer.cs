using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace ZptSharp.Dom
{
    /// <summary>
    /// Implementation of <see cref="IReplacesNode"/> which replaces a node (upon
    /// its parent) using a collection of replacement nodes.
    /// </summary>
    public class NodeReplacer : IReplacesNode
    {
        readonly ILogger logger;

        /// <summary>
        /// Replace the specified node with a collection of replacements.
        /// </summary>
        /// <param name="toReplace">The node to replace.</param>
        /// <param name="replacements">The replacements.</param>
        public void Replace(INode toReplace, IList<INode> replacements)
        {
            if (toReplace == null)
                throw new ArgumentNullException(nameof(toReplace));
            if (replacements == null)
                throw new ArgumentNullException(nameof(replacements));

            var parent = toReplace.ParentElement;

            if (parent == null
                && toReplace.Document is ICanReplaceRootElement rootReplacer
                && replacements.Count == 1)
            {
                rootReplacer.ReplaceRootElement(replacements.Single());
            }
            else if(parent == null)
            {
                var message = String.Format(Resources.ExceptionMessage.MustNotBeRootElement, toReplace, nameof(toReplace));
                throw new ArgumentException(message);
            }
            else
            {
                ReplaceUsingParent(toReplace, replacements, parent);
            }
        }

        void ReplaceUsingParent(INode toReplace, IList<INode> replacements, INode parent)
        {
            var targetIndex = parent.ChildNodes.IndexOf(toReplace);

            foreach (var replacement in replacements)
                replacement.PreReplacementSourceInfo = toReplace.SourceInfo;

            if (logger.IsEnabled(LogLevel.Trace))
            {
                var replacementsString = String.Join($",{Environment.NewLine}  ", replacements);

                logger.LogTrace(@"Replacing a node with {child_count} child nodes
  Parent element:{parent}
Replaced element:{to_replace}
    Replacements:[
  {replacements} ]",
                                replacements.Count,
                                parent,
                                toReplace,
                                replacementsString);
            }

            parent.ChildNodes.Remove(toReplace);
            parent.AddChildren(replacements, targetIndex);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeReplacer"/> class.
        /// </summary>
        /// <param name="logger">Logger.</param>
        public NodeReplacer(ILogger<NodeReplacer> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    }
}
