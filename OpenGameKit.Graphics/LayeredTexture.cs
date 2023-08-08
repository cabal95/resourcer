using System.Drawing;

using OpenGameKit.Abstractions;

namespace OpenGameKit.Graphics;

/// <summary>
/// A texture that is composed of one or more other textured layered on top of
/// each other.
/// </summary>
public class LayeredTexture : ITexture
{
    /// <summary>
    /// The textures that make up this texture.
    /// </summary>
    private readonly ITexture[] _textures;

    /// <inheritdoc/>
    public int Width => _textures[0].Width;

    /// <inheritdoc/>
    public int Height => _textures[0].Height;

    /// <summary>
    /// Creates a new instance of <see cref="LayeredTexture"/>.
    /// </summary>
    /// <param name="textures">The textures that make up the layers with the first texture being on the bottom and the last texture being on top.</param>
    public LayeredTexture( params ITexture[] textures )
    {
        if ( textures.Length == 0 )
        {
            throw new ArgumentException( "Must specify at least one texture.", nameof( textures ) );
        }

        _textures = textures;
    }

    /// <inheritdoc/>
    public void Draw( IDrawOperation operation, Rectangle destination )
    {
        for ( int i = 0; i < _textures.Length; i++ )
        {
            _textures[i].Draw( operation, destination );
        }
    }
}
