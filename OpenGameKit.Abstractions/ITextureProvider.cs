namespace OpenGameKit.Abstractions
{
    /// <summary>
    /// Provides textures from various sources for the current platform.
    /// </summary>
    public interface ITextureProvider
    {
        /// <summary>
        /// Creates a single texture from the image data provided by the
        /// stream.
        /// </summary>
        /// <param name="stream">The stream that contains the image data.</param>
        /// <returns>A new texture instance.</returns>
        ITexture LoadTexture( Stream stream );

        /// <summary>
        /// Creates a texture sheet from the image data provided by the
        /// stream.
        /// </summary>
        /// <param name="stream">The stream that contains the image data.</param>
        /// <returns>A new texture sheet instance.</returns>
        ITextureSheet LoadTextureSheet(  Stream stream );
    }
}
