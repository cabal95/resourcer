using SkiaSharp;

namespace OpenGameKit.Graphics;

/// <summary>
/// A scene describes and paints the UI for a section of the game.
/// </summary>
public interface IScene
{
    /// <summary>
    /// The current width of the scene.
    /// </summary>
    int Width { get; }

    /// <summary>
    /// The current height of the scene.
    /// </summary>
    int Height { get; }

    /// <summary>
    /// Sets the size of the entire scene.
    /// </summary>
    /// <param name="width">The width of the scene.</param>
    /// <param name="height">The height of the scene.</param>
    void SetSize( int width, int height );

    /// <summary>
    /// Draws the scene.
    /// </summary>
    /// <param name="canvas">The canvas object that the scene will be drawn onto.</param>
    /// <param name="dirtyRect">The area of the canvas that needs to be re-drawn.</param>
    void Draw( SKCanvas canvas, SKRectI dirtyRect );
}
