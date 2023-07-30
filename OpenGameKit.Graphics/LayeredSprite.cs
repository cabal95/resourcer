using SkiaSharp;

namespace OpenGameKit.Graphics;

/// <summary>
/// A sprite that is composed of one or more other sprites layerd on top of
/// each other.
/// </summary>
public class LayeredSprite : ISprite
{
    /// <summary>
    /// The sprites that make up this sprite.
    /// </summary>
    private readonly ISprite[] _sprites;

    /// <inheritdoc/>
    public int Width => _sprites[0].Width;

    /// <inheritdoc/>
    public int Height => _sprites[0].Height;

    /// <summary>
    /// Creates a new instance of <see cref="LayeredSprite"/>.
    /// </summary>
    /// <param name="sprites">The sprites that make up the layers with the first sprite being on the bottom and the last sprite being on top.</param>
    public LayeredSprite( params ISprite[] sprites )
    {
        if ( sprites.Length == 0 )
        {
            throw new ArgumentException( "Must specify at least one sprite.", nameof( sprites ) );
        }

        _sprites = sprites;
    }

    /// <inheritdoc/>
    public void Draw( IDrawOperation operation, SKRect destination )
    {
        for ( int i = 0; i < _sprites.Length; i++ )
        {
            _sprites[i].Draw( operation, destination );
        }
    }
}
