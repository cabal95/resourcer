using System.Collections;

namespace OpenGameKit.Graphics;

/// <summary>
/// A collection of tiles that can be access like an array.
/// </summary>
public class TileCollection : IReadOnlyList<ITile>
{
    private readonly IReadOnlyList<ITile> _tiles;

    /// <inheritdoc/>
    public int Count => _tiles.Count;

    /// <inheritdoc/>
    public ITile this[int index] => _tiles[index];

    /// <summary>
    /// Creates a new instance of <see cref="TileCollection"/> that contains
    /// a series of tiles.
    /// </summary>
    /// <param name="tiles">The tiles this collection will contain.</param>
    public TileCollection( params ITile[] tiles )
    {
        _tiles = tiles;
    }

    /// <inheritdoc/>
    public IEnumerator<ITile> GetEnumerator()
    {
        return _tiles.GetEnumerator();
    }

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return _tiles.GetEnumerator();
    }
}
