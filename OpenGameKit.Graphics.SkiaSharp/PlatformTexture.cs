using System.Drawing;

using OpenGameKit.Abstractions;

using SkiaSharp;

namespace OpenGameKit.Graphics.SkiaSharp;

/// <summary>
/// A texture that is sourced from the SkiaSharp library.
/// </summary>
internal class PlatformTexture : ITexture
{
    /// <inheritdoc/>
    public int Width { get; }

    /// <inheritdoc/>
    public int Height { get; }

    /// <summary>
    /// The source bitmap that contains the pixel data.
    /// </summary>
    internal SKBitmap Bitmap { get; }

    /// <summary>
    /// The rectangle in <see cref="Bitmap"/> that contains the pixel data for
    /// this texture.
    /// </summary>
    internal SKRect SourceRect { get; }

    /// <summary>
    /// Creates a new instance of <see cref="PlatformTexture"/>.
    /// </summary>
    /// <param name="bitmap">The raw bitmap that contains the pixel data.</param>
    /// <param name="source">The rectangle in the bitmap that contains the pixel data for this texture.</param>
    public PlatformTexture( SKBitmap bitmap, Rectangle source )
    {
        Bitmap = bitmap;
        Width = source.Width;
        Height = source.Height;
        SourceRect = new SKRect( source.Left, source.Top, source.Right, source.Bottom );
    }

    /// <inheritdoc/>
    public virtual void Draw( IDrawOperation operation, Rectangle destination )
    {
        operation.Canvas.DrawTexture( this, destination );
    }
}
