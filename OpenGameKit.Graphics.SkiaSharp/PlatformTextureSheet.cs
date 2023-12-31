﻿using System.Drawing;

using OpenGameKit.Abstractions;

using SkiaSharp;

namespace OpenGameKit.Graphics.SkiaSharp;

/// <summary>
/// A texture set that has no defined structure. Textures are accessed by specific
/// coordinates and sizes.
/// </summary>
internal class PlatformTextureSheet : ITextureSheet
{
    /// <summary>
    /// The source bitmap data.
    /// </summary>
    private readonly SKBitmap _source;

    /// <summary>
    /// Creates a new instance of <see cref="PlatformTextureSheet"/> from the
    /// image represented by <paramref name="stream"/>.
    /// </summary>
    /// <param name="stream">A stream that contains the raw image data.</param>
    public PlatformTextureSheet( Stream stream )
    {
        _source = SKBitmap.Decode( stream );
        _source.SetImmutable();
    }

    /// <inheritdoc/>
    public ITexture GetTextureAt( int x, int y, int width, int height )
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

        return new PlatformTexture( _source, rect );
    }
}
