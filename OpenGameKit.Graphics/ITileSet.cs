using SkiaSharp;

namespace OpenGameKit.Graphics;

/// <summary>
/// A set of tiles that are contained in a single image.
/// </summary>
public interface ITileSet
{
    /// <summary>
    /// Draws a single tile from the set onto the canvas inside the
    /// destination rectangle.
    /// </summary>
    /// <param name="tile">The tile to be drawn from this set.</param>
    /// <param name="canvas">The canvas surface the tile will be painted on.</param>
    /// <param name="destination">The rectangle on the surface that should be filled with the tile.</param>
    void DrawTile( ITile tile, SKCanvas canvas, SKRect destination );
}
