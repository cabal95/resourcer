using System.Drawing;

namespace OpenGameKit.Abstractions;

/// <summary>
/// A canvas is a drawing surface that supports various operations for renering
/// content.
/// </summary>
public interface ICanvas
{
    /// <summary>
    /// Draw the entire sprite into the destination rectangle.
    /// </summary>
    /// <param name="sprite">The sprite to be drawn.</param>
    /// <param name="destination">The rectangle to draw the sprite into.</param>
    void DrawSprite( ISprite sprite, Rectangle destination );
}
