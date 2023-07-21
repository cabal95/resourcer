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

        public GameScene()
        {
            using ( Stream stream = GetType().Assembly.GetManifestResourceStream( "GameMap.Resources.Embedded.overworld.png" ) )
            {
                _tileset = SKBitmap.Decode( stream );
                var x = _tileset.IsImmutable;
                _tileset.SetImmutable();
            }

            var bm = new SKBitmap( 16 * 5, 16, true );
            using ( var c = new SKCanvas( bm ) )
            {
                c.DrawBitmap( _tileset, GetTileRect( 0, 0 ), GetTileRect( 0, 0 ) );
                c.DrawBitmap( _tileset, GetTileRect( 7, 9 ), GetTileRect( 1, 0 ) );
                c.DrawBitmap( _tileset, GetTileRect( 8, 9 ), GetTileRect( 2, 0 ) );
                c.DrawBitmap( _tileset, GetTileRect( 7, 10 ), GetTileRect( 3, 0 ) );
                c.DrawBitmap( _tileset, GetTileRect( 8, 10 ), GetTileRect( 4, 0 ) );
            }

            bm.SetImmutable();
            _grassTiles = bm;
            
        }

        private static SKRect GetTileRect( int x, int y )
        {
            return new SKRect( x * 16, y * 16, ( x + 1 ) * 16, ( y + 1 ) * 16 );
        }


        public void Draw( SKCanvas canvas, RectF dirtyRect )
        {
            var start = DateTime.UtcNow;

            int top = ( int ) Math.Floor( dirtyRect.Top ) - ( ( int ) Math.Floor( dirtyRect.Top ) % 64 );
            int left = ( int ) Math.Floor( dirtyRect.Left ) - ( ( int ) Math.Floor( dirtyRect.Left ) % 64 );

            for ( int y = top; y < dirtyRect.Bottom; y += 64 )
            {
                for ( int x = left; x < dirtyRect.Right; x += 64 )
                {
                    var cellX = ( x - ( ( int ) Math.Floor( dirtyRect.Top ) % 64 ) / 64 );
                    var src = GetTileRect( cellX % 5, 0 );

                    canvas.DrawBitmap( _grassTiles, src, new SKRect( x + Offset.X, y + Offset.Y, x + Offset.X + 64, y + Offset.Y + 64 ) );
                }
            }

            var end = DateTime.UtcNow;
            System.Diagnostics.Debug.WriteLine( $"Draw took {( end - start ).TotalMilliseconds}ms." );
        }
    }
}
