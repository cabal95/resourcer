using System.Drawing;

namespace OpenGameKit.Abstractions;

/// <summary>
/// An object that can be drawn onto a canvas.
/// </summary>
public interface ITexture
{
    /// <summary>
    /// The native width of the texture.
    /// </summary>
    int Width { get; }

    /// <summary>
    /// The native height of the texture.
    /// </summary>
    int Height { get; }

    /// <summary>
    /// Draws the object onto the canvas inside the destination rectangle.
    /// </summary>
    /// <param name="operation">The current drawing operation.</param>
    /// <param name="destination">The rectangle on the surface that should be filled with this instance.</param>
    void Draw( IDrawOperation operation, Rectangle destination );
}
