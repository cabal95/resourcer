using System.Drawing;

namespace OpenGameKit.Abstractions;

/// <summary>
/// A canvas is a drawing surface that supports various operations for renering
/// content.
/// </summary>
public interface ICanvas
{
    /// <summary>
    /// Draw the entire texture into the destination rectangle.
    /// </summary>
    /// <param name="texture">The texture to be drawn.</param>
    /// <param name="destination">The rectangle to draw the texture into.</param>
    void DrawTexture( ITexture texture, Rectangle destination );
}
