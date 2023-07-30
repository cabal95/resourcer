using SkiaSharp;

namespace OpenGameKit.Graphics;

/// <summary>
/// Basic UI element that provides default implementations.
/// </summary>
public abstract class Element : IElement
{
    /// <inheritdoc/>
    public SKRectI Frame { get; set; }

    /// <inheritdoc/>
    public SKSizeI GetDesiredSize( int widthConstraint, int heightConstraint )
    {
        return new SKSizeI( widthConstraint, heightConstraint );
    }

    /// <inheritdoc/>
    public abstract void Draw( SKCanvas canvas );
}
