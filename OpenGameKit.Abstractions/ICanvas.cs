using System.Drawing;

namespace OpenGameKit.Abstractions;

/// <summary>
/// A canvas is a drawing surface that supports various operations for renering
/// content.
/// </summary>
public interface ICanvas
{
    /// <summary>
    /// Saves the state of the canvas. When the state is restored all changes
    /// made will be restored back to their original values.
    /// </summary>
    /// <returns>An object that will restore the state when disposed.</returns>
    IDisposable SaveState();

    /// <summary>
    /// Sets the alpha value for all drawing operations.
    /// </summary>
    /// <param name="alpha">The alpha value between 0 and 1.</param>
    void SetAlpha( float alpha );

    /// <summary>
    /// Applies a scaling factor to all drawing operations.
    /// </summary>
    /// <param name="scale">The scaling factor to apply.</param>
    void Scale( float scale );

    /// <summary>
    /// Translates all operations by the given pixel amounts.
    /// </summary>
    /// <param name="x">The number of pixels to translate horizontally.</param>
    /// <param name="y">The number of pixels to translate vertically.</param>
    void Translate( float x, float y );

    /// <summary>
    /// Clips any drawing operations to be constrained by the given bounds.
    /// </summary>
    /// <param name="bounds">The clipping rectangle.</param>
    void ClipRect( RectangleF bounds );

    #region Drawing Methods

    /// <summary>
    /// Draws a solid rectangle.
    /// </summary>
    /// <param name="rect">The rectangle to be drawn.</param>
    /// <param name="paint">The details on how to pain the rectangle.</param>
    void DrawRect( Rectangle rect, PaintOperation paint );

    /// <summary>
    /// Draw the entire texture into the destination rectangle.
    /// </summary>
    /// <param name="texture">The texture to be drawn.</param>
    /// <param name="destination">The rectangle to draw the texture into.</param>
    void DrawTexture( ITexture texture, Rectangle destination );

    /// <summary>
    /// Draws a string of text at the specified point using the current canvas
    /// settings for colors.
    /// </summary>
    /// <param name="text">The text to be drawn.</param>
    /// <param name="location">The top left corner of the text block.</param>
    /// <param name="paint">The details on how to paint the text.</param>
    void DrawText( string text, Point location, TextPaintOperation paint );

    #endregion
}
