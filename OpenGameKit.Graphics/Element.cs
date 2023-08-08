using System.Drawing;

using OpenGameKit.Abstractions;

namespace OpenGameKit.Graphics;

/// <summary>
/// Basic UI element that provides default implementations.
/// </summary>
public abstract class Element : IElement
{
    /// <inheritdoc/>
    public Rectangle Frame { get; set; }

    /// <inheritdoc/>
    public virtual Size GetDesiredSize( int widthConstraint, int heightConstraint )
    {
        return new Size( widthConstraint, heightConstraint );
    }

    /// <inheritdoc/>
    public abstract void Draw( IDrawOperation operation );
}
