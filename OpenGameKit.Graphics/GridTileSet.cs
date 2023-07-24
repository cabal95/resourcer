using SkiaSharp;

namespace OpenGameKit.Graphics;

/// <summary>
/// A tile set that is layed out in a grid of equally sized tiles.
/// </summary>
public class GridTileSet : ITileSet
{
    /// <summary>
    /// The source bitmap data.
    /// </summary>
    private readonly SKBitmap _source;

    /// <summary>
    /// The width of each tile.
    /// </summary>
    private readonly int _tileWidth;

    /// <summary>
    /// The height of each tile.
    /// </summary>
    private readonly int _tileHeight;

    /// <summary>
    /// The number of tiles in the X axis.
    /// </summary>
    private readonly int _gridWidth;

    /// <summary>
    /// The number of tiles in the Y axis.
    /// </summary>
    private readonly int _gridHeight;

    /// <summary>
    /// Creates a new instance of <see cref="GridTileSet"/> from the image
    /// represented by <paramref name="stream"/>.
    /// </summary>
    /// <param name="stream">A stream that contains the raw image data.</param>
    /// <param name="tileWidth">The width of each tile.</param>
    /// <param name="tileHeight">The height of each tile.</param>
    public GridTileSet( Stream stream, int tileWidth, int tileHeight )
    {
        _source = SKBitmap.Decode( stream );
        _source.SetImmutable();

        _tileWidth = tileWidth;
        _tileHeight = tileHeight;
        _gridWidth = _source.Width / tileWidth;
        _gridHeight = _source.Height / tileWidth;
    }

    /// <summary>
    /// Gets the tile at the specified grid position.
    /// </summary>
    /// <param name="x">The zero based index of the tile from the left edge of the grid.</param>
    /// <param name="y">The zero based index of the tile from the top edge of the grid.</param>
    /// <returns>A reference to the tile.</returns>
    public ITile GetTileAt( int x, int y )
    {
        if ( x < 0 || x >= _gridWidth || x >= 4096 )
        {
            throw new ArgumentOutOfRangeException( nameof( x ) );
        }

        if ( y < 0 || y >= _gridHeight || y >= 4096 )
        {
            throw new ArgumentOutOfRangeException( nameof( y ) );
        }

        var id = x + ( y << 12 );

        return new TileSprite( this, id, x, y );
    }

    /// <inheritdoc/>
    public void DrawTile( ITile tile, SKCanvas canvas, SKRect destination )
    {
        var id = tile.Id;
        var x = id & 0xfff;
        var y = ( id >> 12 ) & 0xff;

        var src = new SKRect( x * _tileWidth, ( y * _tileHeight ), ( x + 1 ) * _tileWidth, ( y + 1 ) * _tileHeight );

        canvas.DrawBitmap( _source, src, destination );
    }
}
