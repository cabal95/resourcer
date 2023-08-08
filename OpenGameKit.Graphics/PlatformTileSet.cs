using System.Drawing;

using SkiaSharp;

namespace OpenGameKit.Graphics;

/// <summary>
/// A tile set that has no defined structure. Tiles are accessed by specific
/// coordinates and sizes.
/// </summary>
public class PlatformTileSet : ITileSet
{
    /// <summary>
    /// The source bitmap data.
    /// </summary>
    private readonly SKBitmap _source;

    /// <summary>
    /// Creates a new instance of <see cref="PlatformTileSet"/> from the
    /// image represented by <paramref name="stream"/>.
    /// </summary>
    /// <param name="stream">A stream that contains the raw image data.</param>
    public PlatformTileSet( Stream stream )
    {
        _source = SKBitmap.Decode( stream );
        _source.SetImmutable();
    }

    /// <inheritdoc/>
    public ISprite GetSpriteAt( int x, int y, int width, int height )
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

        var rect = new Rectangle( x, y, width, height );

        return new PlatformSprite( _source, rect );
    }
}
