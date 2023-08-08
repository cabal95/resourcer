using OpenGameKit.Abstractions;

namespace OpenGameKit.Graphics;

/// <summary>
/// A tile set that is layed out in a grid of equally sized tiles.
/// </summary>
public class GridTileSet : ITileSet
{
    /// <summary>
    /// The source tile set that we will use to retrieve the tiles.
    /// </summary>
    private readonly ITileSet _source;

    /// <summary>
    /// The width of each tile.
    /// </summary>
    private readonly int _tileWidth;

    /// <summary>
    /// The height of each tile.
    /// </summary>
    private readonly int _tileHeight;

    /// <summary>
    /// Creates a new instance of <see cref="GridTileSet"/>.
    /// </summary>
    /// <param name="sourceTileSet">A tile set that provides the real tiles.</param>
    /// <param name="tileWidth">The width of each tile.</param>
    /// <param name="tileHeight">The height of each tile.</param>
    public GridTileSet( ITileSet sourceTileSet, int tileWidth, int tileHeight )
    {
        _source = sourceTileSet;

        _tileWidth = tileWidth;
        _tileHeight = tileHeight;
    }

    /// <inheritdoc/>
    public ISprite GetSpriteAt( int x, int y, int width, int height )
    {
        return _source.GetSpriteAt( x, y, width, height );
    }

    /// <summary>
    /// Gets the tile at the specified grid position.
    /// </summary>
    /// <param name="x">The zero based index of the tile from the left edge of the grid.</param>
    /// <param name="y">The zero based index of the tile from the top edge of the grid.</param>
    /// <returns>A reference to the tile.</returns>
    public ISprite GetSpriteAt( int x, int y )
    {
        if ( x < 0 )
        {
            throw new ArgumentOutOfRangeException( nameof( x ) );
        }

        if ( y < 0 )
        {
            throw new ArgumentOutOfRangeException( nameof( y ) );
        }

        return _source.GetSpriteAt( x * _tileWidth, y * _tileHeight, _tileWidth, _tileHeight );
    }
}
