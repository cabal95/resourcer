using SkiaSharp;

namespace OpenGameKit.Graphics;

/// <summary>
/// A tile set that has no defined structure. Tiles are accessed by specific
/// coordinates and sizes.
/// </summary>
public class UnstructuredTileSet : ITileSet
{
    /// <summary>
    /// The source bitmap data.
    /// </summary>
    private readonly SKBitmap _source;

    /// <summary>
    /// Lookup table for tiles and their positions.
    /// </summary>
    private readonly Dictionary<int, SKRectI> _tiles = new();

    /// <summary>
    /// The next tile identifier to use when a tile is requested.
    /// </summary>
    private int _nextTileId = 0;

    /// <summary>
    /// Creates a new instance of <see cref="UnstructuredTileSet"/> from the
    /// image represented by <paramref name="stream"/>.
    /// </summary>
    /// <param name="stream">A stream that contains the raw image data.</param>
    public UnstructuredTileSet( Stream stream )
    {
        _source = SKBitmap.Decode( stream );
        _source.SetImmutable();
    }

    /// <summary>
    /// Gets the tile at the specified pixel position.
    /// </summary>
    /// <param name="x">The zero based index of the tile from the left edge of the image.</param>
    /// <param name="y">The zero based index of the tile from the top edge of the image.</param>
    /// <param name="width">The width of the tile.</param>
    /// <param name="height">The height of the tile.</param>
    /// <returns>A reference to the tile.</returns>
    public ITile GetTileAt( int x, int y, int width, int height )
    {
        if ( x < 0 || x + width > _source.Width )
        {
            throw new ArgumentOutOfRangeException( nameof( x ) );
        }

        if ( y < 0 || y + height > _source.Height )
        {
            throw new ArgumentOutOfRangeException( nameof( y ) );
        }

        if ( width <= 0 )
        {
            throw new ArgumentOutOfRangeException( nameof( width ) );
        }

        if ( height <= 0 )
        {
            throw new ArgumentOutOfRangeException( nameof( height ) );
        }

        var id = _nextTileId++;
        var rect = new SKRectI( x, y, x + width, y + height );

        _tiles.Add( id, rect );

        return new TileSprite( this, id, x, y );
    }

    /// <inheritdoc/>
    public void DrawTile( ITile tile, SKCanvas canvas, SKRect destination )
    {
        if ( !_tiles.TryGetValue( tile.Id, out var src ) )
        {
            throw new ArgumentOutOfRangeException( nameof( tile ), "Tile does not belong to this tile set." );
        }

        canvas.DrawBitmap( _source, src, destination );
    }
}
