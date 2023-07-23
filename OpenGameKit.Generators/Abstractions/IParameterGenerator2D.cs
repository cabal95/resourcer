namespace OpenGameKit.Generators.Abstractions;

/// <summary>
/// A single parameter generator for a 2D coordinate plane. Uses the set of
/// waves to determine the characteristics of the value.
/// </summary>
public interface IParameterGenerator2D
{
    /// <summary>
    /// Generates the value at the given coordinates using the wave parameter
    /// data for the variable calculations.
    /// </summary>
    /// <param name="x">The x-coordinate on the map.</param>
    /// <param name="y">The y-coordinate on the map.</param>
    /// <param name="waves">The wave parameter data.</param>
    /// <returns>A deterministic randomized value between 0 and 1.</returns>
    float Generate( int x, int y, IReadOnlyList<IParameterWave> waves );
}
