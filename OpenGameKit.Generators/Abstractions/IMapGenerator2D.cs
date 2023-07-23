namespace OpenGameKit.Generators.Abstractions;

/// <summary>
/// Generates a map of the given cell types in 2D coordinate space.
/// </summary>
/// <typeparam name="TCell">The type of cell that describes the generated map.</typeparam>
public interface IMapGenerator2D<TCell>
{
    /// <summary>
    /// Creates a map chunk at the specified coordinates.
    /// </summary>
    /// <param name="x">The starting X coordinate of the map.</param>
    /// <param name="y">The starting Y coordinate of the map.</param>
    /// <param name="width">The width of the map chunk.</param>
    /// <param name="height">The height of the map chunk.</param>
    /// <returns>A 2D array of the cell objects that represent the map.</returns>
    TCell[,] CreateMap( int x, int y, uint width, uint height );
}
