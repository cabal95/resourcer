using OpenGameKit.Abstractions;

namespace OpenGameKit.Graphics;

/// <summary>
/// A texture sheet that is layed out in a grid of equally sized images.
/// </summary>
public class GridTextureSheet : ITextureSheet
{
    /// <summary>
    /// The source texture set that we will use to retrieve the textures.
    /// </summary>
    private readonly ITextureSheet _source;

    /// <summary>
    /// The width of each texture.
    /// </summary>
    private readonly int _textureWidth;

    /// <summary>
    /// The height of each texture.
    /// </summary>
    private readonly int _textureHeight;

    /// <summary>
    /// Creates a new instance of <see cref="GridTextureSheet"/>.
    /// </summary>
    /// <param name="sourceTextureSheet">A texture set that provides the real textures.</param>
    /// <param name="textureWidth">The width of each texture.</param>
    /// <param name="textureHeight">The height of each texture.</param>
    public GridTextureSheet( ITextureSheet sourceTextureSheet, int textureWidth, int textureHeight )
    {
        _source = sourceTextureSheet;

        _textureWidth = textureWidth;
        _textureHeight = textureHeight;
    }

    /// <inheritdoc/>
    public ITexture GetTextureAt( int x, int y, int width, int height )
    {
        return _source.GetTextureAt( x, y, width, height );
    }

    /// <summary>
    /// Gets the texture at the specified grid position.
    /// </summary>
    /// <param name="x">The zero based index of the texture from the left edge of the grid.</param>
    /// <param name="y">The zero based index of the texture from the top edge of the grid.</param>
    /// <returns>A reference to the texture.</returns>
    public ITexture GetTextureAt( int x, int y )
    {
        if ( x < 0 )
        {
            throw new ArgumentOutOfRangeException( nameof( x ) );
        }

        if ( y < 0 )
        {
            throw new ArgumentOutOfRangeException( nameof( y ) );
        }

        return _source.GetTextureAt( x * _textureWidth, y * _textureHeight, _textureWidth, _textureHeight );
    }
}
