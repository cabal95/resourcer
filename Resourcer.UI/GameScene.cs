using Resourcer.UI;

using SkiaSharp;

namespace Resourcer
{
    public interface IScene
    {
        void Draw( SKCanvas canvas, SKRect dirtyRect );
    }

    public class GameScene : IScene
    {
        public SKPoint Offset { get; set; } = SKPoint.Empty;

        private SpriteProvider _sprites;

        public GameScene( SpriteProvider sprites )
        {
            _sprites = sprites;
        }

        public void Draw( SKCanvas canvas, SKRect dirtyRect )
        {
            var start = DateTime.UtcNow;

            // Determine the X,Y map coordinates we will start painting from.
            var mapLeft = ( int ) Math.Floor( ( Offset.X + dirtyRect.Left ) / 64 );
            var mapTop = ( int ) Math.Floor( ( Offset.Y + dirtyRect.Top ) / 64 );

            var offsetXRemainder = ( int ) Math.Floor( Offset.X ) % 64;
            var offsetYRemainder = ( int ) Math.Floor( Offset.Y ) % 64;

            // Determine the starting painting position.
            int left = ( ( ( int ) Math.Floor( dirtyRect.Left / 64 ) ) * 64 );
            int top = ( ( int ) Math.Floor( dirtyRect.Top / 64 ) ) * 64;

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
                    //var cellX = ( x - ( ( int ) Math.Floor( dirtyRect.Top ) % 64 ) / 64 );
                    //var src = GetTileRect( cellX % 5, 0 );

                    //canvas.DrawBitmap( _grassTiles, src, new SKRect( x + Offset.X, y + Offset.Y, x + Offset.X + 64, y + Offset.Y + 64 ) );
                }
            }

            var end = DateTime.UtcNow;
            System.Diagnostics.Debug.WriteLine( $"Draw took {( end - start ).TotalMilliseconds}ms." );
        }
    }
}
