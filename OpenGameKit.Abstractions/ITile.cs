namespace OpenGameKit.Abstractions;

/// <summary>
/// Represents a single tile from a <see cref="ITileSet"/>.
/// </summary>
public interface ITile : ISprite
{
    /// <summary>
    /// The identifier of the tile.
    /// </summary>
    int Id { get; }
}
