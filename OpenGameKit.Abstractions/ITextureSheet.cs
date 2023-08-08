namespace OpenGameKit.Abstractions;

/// <summary>
/// A set of textures that are contained in a single image.
/// </summary>
public interface ITextureSheet
{
    /// <summary>
    /// Gets the sprite at the specified pixel position.
    /// </summary>
    /// <param name="x">The zero based index of the texture from the left edge of the image.</param>
    /// <param name="y">The zero based index of the texture from the top edge of the image.</param>
    /// <param name="width">The width of the texture.</param>
    /// <param name="height">The height of the texture.</param>
    /// <returns>A reference to the texture.</returns>
    ITexture GetTextureAt( int x, int y, int width, int height );
}
