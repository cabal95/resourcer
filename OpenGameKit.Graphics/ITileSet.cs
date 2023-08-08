namespace OpenGameKit.Graphics;

/// <summary>
/// A set of tiles that are contained in a single image.
/// </summary>
public interface ITileSet
{
    /// <summary>
    /// Gets the sprite at the specified pixel position.
    /// </summary>
    /// <param name="x">The zero based index of the tile from the left edge of the image.</param>
    /// <param name="y">The zero based index of the tile from the top edge of the image.</param>
    /// <param name="width">The width of the tile.</param>
    /// <param name="height">The height of the tile.</param>
    /// <returns>A reference to the tile.</returns>
    ISprite GetSpriteAt( int x, int y, int width, int height );
}
