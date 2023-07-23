using OpenGameKit.Generators.Abstractions;

namespace OpenGameKit.Generators
{
    /// <summary>
    /// The default implementation of a paremter.
    /// </summary>
    public class ParameterDescription : IParameterDescription
    {
        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public IReadOnlyList<IParameterWave> Waves { get; }

        /// <summary>
        /// Creates a new instance of <see cref="ParameterDescription"/>.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="waves">The waves that will be used to generate the cell data.</param>
        public ParameterDescription( string name, IReadOnlyList<IParameterWave> waves )
        {
            Name = name;
            Waves = waves;
        }
    }
}
