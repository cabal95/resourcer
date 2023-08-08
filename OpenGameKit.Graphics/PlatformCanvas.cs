using System.Drawing;

using SkiaSharp;

namespace OpenGameKit.Graphics;

/// <summary>
/// A canvas that supports drawing operations to SkiaSharp.
/// </summary>
public class PlatformCanvas : ICanvas
{
    /// <summary>
    /// The native canvas objec.t
    /// </summary>
    private readonly SKCanvas _canvas;

    /// <summary>
    /// Creates a new instance of <see cref="PlatformCanvas"/>.
    /// </summary>
    /// <param name="canvas">The native SkiaSharp canvas.</param>
    public PlatformCanvas( SKCanvas canvas )
    {
        _canvas = canvas;
    }

    /// <inheritdoc/>
    public void DrawSprite( ISprite sprite, Rectangle destination )
    {
        if ( sprite is not PlatformSprite platformSprite )
        {
            throw new ArgumentException( $"{nameof( DrawSprite )} must be called with a PlatformSprite.", nameof( sprite ) );
        }

        var skiaDestination = new SKRect( destination.Left, destination.Top, destination.Right, destination.Bottom );

        _canvas.DrawBitmap( platformSprite.Bitmap, platformSprite.SourceRect, skiaDestination );
    }
}
