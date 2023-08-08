using System.Drawing;

using OpenGameKit.Abstractions;
using OpenGameKit.Graphics.SkiaSharp;

using SkiaSharp;

namespace OpenGameKit.Graphics;

/// <summary>
/// A canvas that supports drawing operations to SkiaSharp.
/// </summary>
public class SkiaSharpCanvas : ICanvas
{
    /// <summary>
    /// The native canvas object.
    /// </summary>
    private readonly SKCanvas _canvas;

    /// <summary>
    /// Creates a new instance of <see cref="SkiaSharpCanvas"/>.
    /// </summary>
    /// <param name="canvas">The native SkiaSharp canvas.</param>
    public SkiaSharpCanvas( SKCanvas canvas )
    {
        _canvas = canvas;
    }

    /// <inheritdoc/>
    public void DrawTexture( ITexture texture, Rectangle destination )
    {
        if ( texture is not PlatformTexture platformTexture )
        {
            throw new ArgumentException( $"{nameof( DrawTexture )} must be called with a ${nameof( PlatformTexture )}.", nameof( texture ) );
        }

        var skiaDestination = new SKRect( destination.Left, destination.Top, destination.Right, destination.Bottom );

        _canvas.DrawBitmap( platformTexture.Bitmap, platformTexture.SourceRect, skiaDestination );
    }
}
