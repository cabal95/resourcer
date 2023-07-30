using Microsoft.Extensions.DependencyInjection;

using SkiaSharp;

namespace OpenGameKit.Graphics;

/// <summary>
/// A representation of a single tile from a <see cref="ITileSet"/>.
/// </summary>
public class TileSprite : ITile
{
    /// <summary>
    /// The tile set that created this sprite.
    /// </summary>
    private readonly ITileSet _tileSet;

    /// <inheritdoc/>
    public int Id { get; }

    /// <inheritdoc/>
    public int Width { get; }

    /// <inheritdoc/>
    public int Height { get; }

    /// <summary>
    /// Creates a new instance of <see cref="TileSprite"/> that can be used to
    /// represent a single tile from any tile set.
    /// </summary>
    /// <param name="tileSet">The tile set that represents the original image.</param>
    /// <param name="id">The identifier of this tile in the set.</param>
    /// <param name="width">The native width of the tile.</param>
    /// <param name="height">The native height of the tile.</param>
    public TileSprite( ITileSet tileSet, int id, int width, int height )
    {
        _tileSet = tileSet;
        Id = id;
        Width = width;
        Height = height;
    }

    /// <inheritdoc/>
    public void Draw( IDrawOperation operation, SKRect destination )
    {
        _tileSet.DrawTile( this, operation.GetRequiredService<SKCanvas>(), destination );
    }
}
