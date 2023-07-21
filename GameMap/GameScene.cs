using SkiaSharp;

namespace GameMap
{
    public interface IScene
    {
        void Draw( SKCanvas canvas, RectF dirtyRect );
    }

    public class GameScene : IScene
    {
        public PointF Offset { get; set; } = Point.Zero;

        private readonly SKBitmap _tileset;
        private readonly SKBitmap _grassTiles;
        private readonly SKBitmap _waterTiles;
        private readonly SKBitmap _tundraTiles;
        private readonly SKBitmap _mountainTiles;
        private readonly SKBitmap _desertTiles;
        private readonly SKBitmap _forestTiles;

        private readonly byte[,] _map;

        public GameScene()
        {
            using ( Stream stream = GetType().Assembly.GetManifestResourceStream( "GameMap.Resources.Embedded.overworld.png" ) )
            {
                _tileset = SKBitmap.Decode( stream );
                var x = _tileset.IsImmutable;
                _tileset.SetImmutable();
            }

            _grassTiles = new SKBitmap( 16 * 5, 16, true );
            using ( var c = new SKCanvas( _grassTiles ) )
            {
                c.DrawBitmap( _tileset, GetTileRect( 0, 0 ), GetTileRect( 0, 0 ) );
                c.DrawBitmap( _tileset, GetTileRect( 7, 9 ), GetTileRect( 1, 0 ) );
                c.DrawBitmap( _tileset, GetTileRect( 8, 9 ), GetTileRect( 2, 0 ) );
                c.DrawBitmap( _tileset, GetTileRect( 7, 10 ), GetTileRect( 3, 0 ) );
                c.DrawBitmap( _tileset, GetTileRect( 8, 10 ), GetTileRect( 4, 0 ) );
            }
            _grassTiles.SetImmutable();

            _waterTiles = new SKBitmap( 16 * 8, 16, true );
            using ( var c = new SKCanvas( _waterTiles ) )
            {
                c.DrawBitmap( _tileset, GetTileRect( 0, 1 ), GetTileRect( 0, 0 ) );
                c.DrawBitmap( _tileset, GetTileRect( 1, 1 ), GetTileRect( 1, 0 ) );
                c.DrawBitmap( _tileset, GetTileRect( 2, 1 ), GetTileRect( 2, 0 ) );
                c.DrawBitmap( _tileset, GetTileRect( 3, 1 ), GetTileRect( 3, 0 ) );
                c.DrawBitmap( _tileset, GetTileRect( 0, 2 ), GetTileRect( 4, 0 ) );
                c.DrawBitmap( _tileset, GetTileRect( 1, 2 ), GetTileRect( 5, 0 ) );
                c.DrawBitmap( _tileset, GetTileRect( 2, 2 ), GetTileRect( 6, 0 ) );
                c.DrawBitmap( _tileset, GetTileRect( 3, 2 ), GetTileRect( 7, 0 ) );
            }
            _waterTiles.SetImmutable();

            _tundraTiles = new SKBitmap( 16 * 2, 16, true );
            using ( var c = new SKCanvas( _tundraTiles ) )
            {
                c.DrawBitmap( _tileset, GetTileRect( 14, 11 ), GetTileRect( 0, 0 ) );
                c.DrawBitmap( _tileset, GetTileRect( 14, 12 ), GetTileRect( 1, 0 ) );
            }
            _tundraTiles.SetImmutable();

            _mountainTiles = new SKBitmap( 16 * 1, 16, true );
            using ( var c = new SKCanvas( _mountainTiles ) )
            {
                c.DrawBitmap( _tileset, GetTileRect( 0, 0 ), GetTileRect( 0, 0 ) );
                c.DrawBitmap( _tileset, GetTileRect( 7, 5 ), GetTileRect( 0, 0 ) );
            }
            _mountainTiles.SetImmutable();

            _desertTiles = new SKBitmap( 16 * 1, 16, true );
            using ( var c = new SKCanvas( _desertTiles ) )
            {
                c.DrawBitmap( _tileset, GetTileRect( 2, 32 ), GetTileRect( 0, 0 ) );
            }
            _desertTiles.SetImmutable();

            _forestTiles = new SKBitmap( 16 * 1, 16, true );
            using ( var c = new SKCanvas( _forestTiles ) )
            {
                c.DrawBitmap( _tileset, GetTileRect( 0, 0 ), GetTileRect( 0, 0 ) );
                c.DrawBitmap( _tileset, GetTileRect( 2, 14 ), GetTileRect( 0, 0 ) );
            }
            _forestTiles.SetImmutable();

            var generator = new MapGenerator.BiomeGenerator
            {
                Width = 256,
                Height = 256
            };
            _map = generator.GenerateMap( 1234 );
            
        }

        private static SKRect GetTileRect( int x, int y )
        {
            return new SKRect( x * 16, y * 16, ( x + 1 ) * 16, ( y + 1 ) * 16 );
        }


        public void Draw( SKCanvas canvas, RectF dirtyRect )
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
                        var biome = _map[mapX, mapY];
                        if ( biome == 'G' )
                        {
                            canvas.DrawBitmap( _grassTiles, GetTileRect( 0, 0 ), new SKRect( x, y, x + 64, y + 64 ) );
                        }
                        else if ( biome == 'W' )
                        {
                            canvas.DrawBitmap( _waterTiles, GetTileRect( 0, 0 ), new SKRect( x, y, x + 64, y + 64 ) );
                        }
                        else if ( biome == 'T' )
                        {
                            canvas.DrawBitmap( _tundraTiles, GetTileRect( 0, 0 ), new SKRect( x, y, x + 64, y + 64 ) );
                        }
                        else if ( biome == 'M' )
                        {
                            canvas.DrawBitmap( _mountainTiles, GetTileRect( 0, 0 ), new SKRect( x, y, x + 64, y + 64 ) );
                        }
                        else if ( biome == 'D' )
                        {
                            canvas.DrawBitmap( _desertTiles, GetTileRect( 0, 0 ), new SKRect( x, y, x + 64, y + 64 ) );
                        }
                        else if ( biome == 'F' )
                        {
                            canvas.DrawBitmap( _forestTiles, GetTileRect( 0, 0 ), new SKRect( x, y, x + 64, y + 64 ) );
                        }
                        else
                        {

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
