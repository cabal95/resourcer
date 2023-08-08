namespace OpenGameKit.Abstractions;

/// <summary>
/// A map generator that uses parameterized descriptions to generate a set of
/// 2D map values with those parameter values.
/// </summary>
public interface IMapParameterGenerator2D
{
    /// <summary>
    /// Generates a set of values for the map chunk specified by the starting
    /// coordinates of the given size. This returns a 2D array of dictionaries
    /// that represent the generated values.
    /// </summary>
    /// <param name="x">The starting X coordinate of the map chunk.</param>
    /// <param name="y">The starting Y coordinate of the map chunk.</param>
    /// <param name="width">The width of the map chunk to generate.</param>
    /// <param name="height">The height of the map chunk to generate.</param>
    /// <param name="parameters">The parameters that describe how to generate the randomized values.</param>
    /// <returns>A 2D array of dictionaries that contain the map cell values.</returns>
    IReadOnlyDictionary<string, float>[,] Generate( int x, int y, uint width, uint height, IReadOnlyList<IParameterDescription> parameters );
}

