using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Dom;

namespace ZptSharp.Expressions
{
    /// <summary>
    /// Provides contextual information about a repetition/iteration.  This is used in looping constructs.
    /// </summary>
    public class RepetitionInfo : IGetsNamedTalesValue
    {
        const string
            index = "index",
            number = "number",
            even = "even",
            odd = "odd",
            start = "start",
            end = "end",
            length = "length",
            letter = "letter",
            Letter = "Letter",
            roman = "roman",
            Roman = "Roman";

        readonly IGetsAlphabeticValueForNumber alphabeticValueProvider;
        readonly IGetsRomanNumeralForNumber romanNumeralProvider;

        /// <summary>
        /// Gets or sets the name of the repetition.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the total number of items in the repetition.
        /// </summary>
        /// <value>The count.</value>
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets the zero-based index of the current iteration.
        /// </summary>
        /// <value>The index of the current iteration.</value>
        public int CurrentIndex { get; set; }

        /// <summary>
        /// Gets or sets the element associated with the repetition.
        /// </summary>
        /// <value>The element.</value>
        public INode Element { get; set; }

        /// <summary>
        /// Gets or sets the value associated with the current iteration.
        /// </summary>
        /// <value>The current value.</value>
        public object CurrentValue { get; set; }

        /// <summary>
        /// Attempts to get a value for a named reference, relative to the current instance.
        /// </summary>
        /// <returns>An object indicating whether a value was successfully retrieved or not, along with the retrieved value (if applicable).</returns>
        /// <param name="name">The name of the value to retrieve.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        public Task<GetValueResult> TryGetValueAsync(string name, CancellationToken cancellationToken = default)
            => Task.FromResult(TryGetValue(name));

        GetValueResult TryGetValue(string name)
        {
            var num = CurrentIndex + 1;

            switch (name)
            {
                case index: return GetValueResult.For(CurrentIndex);
                case number: return GetValueResult.For(num);
                case even: return GetValueResult.For(CurrentIndex % 2 == 0);
                case odd: return GetValueResult.For(CurrentIndex % 2 != 0);
                case start: return GetValueResult.For(CurrentIndex == 0);
                case end: return GetValueResult.For(CurrentIndex == Count - 1);
                case length: return GetValueResult.For(Count);
                case letter: return GetValueResult.For(alphabeticValueProvider.GetAlphabeticValue(CurrentIndex));
                case Letter: return GetValueResult.For(alphabeticValueProvider.GetAlphabeticValue(CurrentIndex).ToUpperInvariant());
                case roman: return GetValueResult.For(romanNumeralProvider.GetRomanNumeral(num).ToLowerInvariant());
                case Roman: return GetValueResult.For(romanNumeralProvider.GetRomanNumeral(num));

                default: return GetValueResult.Failure;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepetitionInfo"/> class.
        /// </summary>
        /// <param name="alphabeticValueProvider">Alphabetic value provider.</param>
        /// <param name="romanNumeralProvider">Roman numeral provider.</param>
        public RepetitionInfo(IGetsAlphabeticValueForNumber alphabeticValueProvider,
                              IGetsRomanNumeralForNumber romanNumeralProvider)
        {
            this.alphabeticValueProvider = alphabeticValueProvider ?? throw new System.ArgumentNullException(nameof(alphabeticValueProvider));
            this.romanNumeralProvider = romanNumeralProvider ?? throw new System.ArgumentNullException(nameof(romanNumeralProvider));
        }
    }
}
