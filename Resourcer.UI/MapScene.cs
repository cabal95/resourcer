using OpenGameKit.Graphics;

using Resourcer.UI;

using SkiaSharp;

namespace Resourcer;

/// <summary>
/// A simple scene that draws the map tile background of the game.
/// </summary>
public class MapScene : IScene
{
    #region Fields

    /// <summary>
    /// The provider for the tile sprites.
    /// </summary>
    private readonly SpriteProvider _sprites;

    #endregion

    #region Properties

    /// <inheritdoc/>
    public SKRectI Frame { get; set; }

    /// <summary>
    /// A representation of the raw pixel position of the top left corner of
    /// the map in the world.
    /// </summary>
    public SKPointI Offset { get; set; } = SKPointI.Empty;

    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="MapScene"/> that will handle
    /// drawing the map tiles.
    /// </summary>
    /// <param name="sprites">The provider for the tile sprites.</param>
    public MapScene( SpriteProvider sprites )
    {
        _sprites = sprites;
    }

    /// <inheritdoc/>
    public SKSizeI GetDesiredSize( int widthConstraint, int heightConstraint )
    {
        return new SKSizeI( widthConstraint, heightConstraint );
    }

    /// <inheritdoc/>
    public void Draw( SKCanvas canvas )
    {
        // Determine the X,Y map coordinates we will start painting from.
        var mapLeft = ( int ) Math.Floor( Offset.X / 64.0 );
        var mapTop = ( int ) Math.Floor( Offset.Y / 64.0 );
        var mapRight = ( int ) Math.Floor( ( Offset.X + Frame.Width ) / 64.0 );
        var mapBottom = ( int ) Math.Floor( ( Offset.Y + Frame.Height ) / 64.0 );

        // Determine the tile offset.
        var tileOffsetX = Offset.X % 64 != 0
            ? Offset.X < 0 ? 64 + ( Offset.X % 64 ) : Offset.X % 64
            : 0;
        var tileOffsetY = Offset.Y % 64 != 0
            ? Offset.Y < 0 ? 64 + ( Offset.Y % 64 ) : Offset.Y % 64
            : 0;

        // Determine the starting painting position.
        int left = Frame.Left - tileOffsetX;
        int top = Frame.Top - tileOffsetY;

        for ( int y = top, mapY = mapTop; mapY <= mapBottom; y += 64, mapY++ )
        {
            for ( int x = left, mapX = mapLeft; mapX <= mapRight; x += 64, mapX++ )
            {
                if ( mapX >= 0 && mapX < 256 && mapY >= 0 && mapY < 256 )
                {
                    var destination = new SKRect( x, y, x + 64, y + 64 );

                    var biome = _sprites.Map[mapX, mapY];

                    if ( biome == 'G' )
                    {
                        var tileX = Math.Abs( mapX ) * Math.Abs( mapY ) % _sprites.GrassTiles.Count;
                        _sprites.GrassTiles[tileX].Draw( canvas, destination );
                    }
                    else if ( biome == 'W' )
                    {
                        _sprites.WaterTiles[0].Draw( canvas, destination );
                    }
                    else if ( biome == 'T' )
                    {
                        _sprites.TundraTiles[0].Draw( canvas, destination );
                    }
                    else if ( biome == 'M' )
                    {
                        _sprites.MountainTiles[0].Draw( canvas, destination );
                    }
                    else if ( biome == 'D' )
                    {
                        _sprites.DesertTiles[0].Draw( canvas, destination );
                    }
                    else if ( biome == 'F' )
                    {
                        _sprites.ForestTiles[0].Draw( canvas, destination );
                    }
                }
            }
        }
    }
}
