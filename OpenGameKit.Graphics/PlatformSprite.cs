using System.Drawing;

using SkiaSharp;

namespace OpenGameKit.Graphics;

/// <summary>
/// A sprite that is sourced from the SkiaSharp library.
/// </summary>
internal class PlatformSprite : ISprite
{
    /// <inheritdoc/>
    public int Width => throw new NotImplementedException();

    /// <inheritdoc/>
    public int Height => throw new NotImplementedException();

    /// <summary>
    /// The source bitmap that contains the pixel data.
    /// </summary>
    internal SKBitmap Bitmap { get; }

    /// <summary>
    /// The rectangle in <see cref="Bitmap"/> that contains the pixel data for
    /// this sprite.
    /// </summary>
    internal SKRect SourceRect { get; }

    /// <summary>
    /// Creates a new instance of <see cref="PlatformSprite"/>.
    /// </summary>
    /// <param name="bitmap">The raw bitmap that contains the pixel data.</param>
    /// <param name="source">The rectangle in the bitmap that contains the pixel data for this sprite.</param>
    public PlatformSprite( SKBitmap bitmap, Rectangle source )
    {
        Bitmap = bitmap;
        SourceRect = new SKRect( source.Left, source.Top, source.Right, source.Bottom );
    }

    /// <inheritdoc/>
    public virtual void Draw( IDrawOperation operation, Rectangle destination )
    {
        operation.Canvas.DrawSprite( this, destination );
    }
}
