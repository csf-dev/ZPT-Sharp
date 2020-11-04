using System;
using ZptSharp.Dom;

namespace ZptSharp.Metal
{
    /// <summary>
    /// A model which may represent either a defined macro slot or content which should fill a macro slot.
    /// </summary>
    public sealed class Slot : IEquatable<Slot>
    {
        /// <summary>
        /// Gets the name of the slot.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; }

        /// <summary>
        /// Gets the DOM element associated with the slot.  This is either the element which
        /// is replaced when the slot is filled, or the element which fills the slot.
        /// </summary>
        /// <value>The element.</value>
        public IElement Element { get; }

        /// <summary>
        /// Determines whether the specified <see cref="Slot"/> is equal to the current <see cref="Slot"/>.
        /// </summary>
        /// <param name="other">The <see cref="Slot"/> to compare with the current <see cref="Slot"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="Slot"/> is equal to the current
        /// <see cref="Slot"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(Slot other) => String.Equals(other?.Name, Name, StringComparison.InvariantCulture);

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="Slot"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="Slot"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="Slot"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj) => Equals(obj as Slot);

        /// <summary>
        /// Serves as a hash function for a <see cref="Slot"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode() => Name.GetHashCode();

        /// <summary>
        /// Initializes a new instance of the <see cref="Slot"/> class.
        /// </summary>
        /// <param name="name">Slot name.</param>
        /// <param name="element">Element.</param>
        public Slot(string name, IElement element)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Element = element ?? throw new ArgumentNullException(nameof(element));
        }
    }
}
