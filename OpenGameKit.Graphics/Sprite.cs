using System.Drawing;

using OpenGameKit.Abstractions;

namespace OpenGameKit.Graphics;

/// <summary>
/// An element that represents a texture image to be positioned and drawn on
/// the canvas.
/// </summary>
public class Sprite : Element
{
    /// <summary>
    /// The texture that represents this element.
    /// </summary>
    private readonly ITexture _texture;

    /// <summary>
    /// Creates a new instance of <see cref="Sprite"/>.
    /// </summary>
    /// <param name="texture">The texture that will be used to draw this element.</param>
    public Sprite( ITexture texture )
    {
        _texture = texture;
    }

    /// <inheritdoc/>
    public override Size GetDesiredSize( int widthConstraint, int heightConstraint )
    {
        return new Size( _texture.Width, _texture.Height );
    }

    /// <inheritdoc/>
    public override void Draw( IDrawOperation operation )
    {
        operation.Canvas.DrawTexture( _texture, Frame );
    }
}
