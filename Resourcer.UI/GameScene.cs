using Resourcer.UI;

using SkiaSharp;

namespace Resourcer
{
    public class GameScene : IScene
    {
        public SKPointI Offset { get; set; } = SKPointI.Empty;

        private readonly SpriteProvider _sprites;

        public GameScene( SpriteProvider sprites )
        {
            _sprites = sprites;
        }

        /// <inheritdoc/>
        public void Draw( SKCanvas canvas, SKSizeI size, SKRectI dirtyRect )
        {
            // Determine the X,Y map coordinates we will start painting from.
            var mapLeft = ( Offset.X + dirtyRect.Left ) / 64;
            var mapTop = ( Offset.Y + dirtyRect.Top ) / 64;

            var offsetXRemainder = Offset.X % 64;
            var offsetYRemainder = Offset.Y % 64;

            // Determine the starting painting position.
            int left = ( dirtyRect.Left / 64 ) * 64;
            int top = ( dirtyRect.Top / 64 ) * 64;

            if ( offsetXRemainder > 0 )
            {
                left -= offsetXRemainder;
            }
            else if ( offsetXRemainder < 0 )
            {
                left -= 64 + offsetXRemainder;
            }

            if ( offsetYRemainder > 0 )
            {
                top -= offsetYRemainder;
            }
            else if ( offsetYRemainder < 0 )
            {
                top -= 64 + offsetYRemainder;
            }

            for ( int y = top, mapY = mapTop; y < dirtyRect.Bottom; y += 64, mapY++ )
            {
                for ( int x = left, mapX = mapLeft; x < dirtyRect.Right; x += 64, mapX++ )
                {
                    if ( mapX >= 0 && mapX < 256 && mapY >= 0 && mapY < 256 )
                    {
                        var destination = new SKRect( x, y, x + 64, y + 64 );

                        var biome = _sprites.Map[mapX, mapY];

                        if ( biome == 'G' )
                        {
                            var tileX = Math.Abs( mapX ) % _sprites.GrassTiles.Count;
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
                        else
                        {

                        }

                        if ( mapX == 10 && mapY == 10 )
                        {
                            _sprites.CharacterTile.Draw( canvas, destination );
                        }
                    }
                }
            }
        }
    }
}
