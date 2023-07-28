using SkiaSharp;

namespace Resourcer
{
    /// <summary>
    /// A scene describes and paints the UI for a section of the game.
    /// </summary>
    public interface IScene
    {
        /// <summary>
        /// Draws the scene.
        /// </summary>
        /// <param name="canvas">The canvas object that the scene will be drawn onto.</param>
        /// <param name="size">The size of the canvas.</param>
        /// <param name="dirtyRect">The area of the canvas that needs to be re-drawn.</param>
        void Draw( SKCanvas canvas, SKSizeI size, SKRectI dirtyRect );
    }
}
