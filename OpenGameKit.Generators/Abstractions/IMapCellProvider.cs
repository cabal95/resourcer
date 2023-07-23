namespace OpenGameKit.Generators.Abstractions;

/// <summary>
/// Provides a map cell given the randomized parameter values.
/// </summary>
/// <typeparam name="TCell">The class that will represent the map cell.</typeparam>
public interface IMapCellProvider<TCell>
{
    /// <summary>
    /// Creates a cell from the set of parameters that were generated.
    /// </summary>
    /// <param name="parameters">The parameters that were generated.</param>
    /// <returns>A new instance of the map cell.</returns>
    TCell CreateCell( IReadOnlyDictionary<string, float> parameters );

    /// <summary>
    /// Gets the parameter descriptions that should be used when generating
    /// the map cells.
    /// </summary>
    /// <returns>A list of parameter descriptions that should be used to build the cell data.</returns>
    IReadOnlyList<IParameterDescription> GetParameters();
}
