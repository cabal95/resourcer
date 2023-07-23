using OpenGameKit.Generators;
using OpenGameKit.Generators.Abstractions;

using Resourcer.Server.Generators;

using SkiaSharp;

namespace Resourcer
{
    public interface ITile
    {
        void Draw( SKCanvas canvas, SKRect destination );
    }

    public class TileSetTile : ITile
    {
        private readonly TileSet _tileSet;

        private readonly int _x;

        private readonly int _y;

        public TileSetTile( TileSet tileSet, int x, int y )
        {
            _tileSet = tileSet;
            _x = x;
            _y = y;
        }

        public void Draw( SKCanvas canvas, SKRect destination )
        {
            _tileSet.DrawTile( _x, _y, canvas, destination );
        }
    }

    public class TileSet
    {
        /// <summary>
        /// The source bitmap data.
        /// </summary>
        private readonly SKBitmap _source;

        /// <summary>
        /// The width and height of each tile.
        /// </summary>
        private readonly int _tileSize;

        private readonly int _offsetY;

        /// <summary>
        /// The number of tiles in the X axis.
        /// </summary>
        private readonly int _tileWidthCount;

        /// <summary>
        /// The number of tiles in the Y axis.
        /// </summary>
        private readonly int _tileHeightCount;

        public TileSet( Stream stream, int tileSize, int offsetY = 0 )
        {
            _source = SKBitmap.Decode( stream );
            _source.SetImmutable();

            _tileSize = tileSize;
            _offsetY = offsetY;
            _tileWidthCount = _source.Width / tileSize;
            _tileHeightCount = _source.Height / tileSize;
        }

        public ITile TileAt( int x, int y )
        {
            return new TileSetTile( this, x, y );
        }

        public void DrawTile( int x, int y, SKCanvas canvas, SKRect destination )
        {
            if ( x < 0 || y < 0 || x >= _tileWidthCount || y >= _tileHeightCount )
            {
                return;
            }

            var src = new SKRect( x * _tileSize, (y * _tileSize) + (_offsetY * y), ( x + 1 ) * _tileSize, ( y + 1 ) * _tileSize );

            canvas.DrawBitmap( _source, src, destination );
        }
    }

    public class TileSeries
    {
        private readonly ITile[] _sourceTiles;

        public int Count => _sourceTiles.Length;

        public TileSeries( params ITile[] sourceTiles )
        {
            _sourceTiles = sourceTiles;
        }

        public void DrawTile( int index, SKCanvas canvas, SKRect destination )
        {
            if ( index < 0 || index >= _sourceTiles.Length )
            {
                return;
            }

            _sourceTiles[index].Draw( canvas, destination );
        }
    }

    public class AdditiveTile : ITile
    {
        private readonly ITile[] _sourceTiles;

        public int Count => _sourceTiles.Length;

        public AdditiveTile( params ITile[] sourceTiles )
        {
            _sourceTiles = sourceTiles;
        }

        public void Draw( SKCanvas canvas, SKRect destination )
        {
            for ( int i = 0; i < _sourceTiles.Length; i++ )
            {
                _sourceTiles[i].Draw( canvas, destination );
            }
        }
    }

    public class AnimatedTile : ITile
    {
        private int _index;

        private readonly ITile[] _sourceTiles;

        public int Count => _sourceTiles.Length;

        public AnimatedTile( params ITile[] sourceTiles )
        {
            _sourceTiles = sourceTiles;
        }

        public void Draw( SKCanvas canvas, SKRect destination )
        {
            if ( _index >= _sourceTiles.Length )
            {
                _index = 0;
            }

            _sourceTiles[_index++].Draw( canvas, destination );
        }
    }


    public interface IScene
    {
        void Draw( SKCanvas canvas, RectF dirtyRect );
    }

    public class GameScene : IScene
    {
        public PointF Offset { get; set; } = Point.Zero;

        private readonly TileSet _overworld;
        private readonly TileSeries _grassTiles;
        private readonly TileSeries _waterTiles;
        private readonly TileSeries _tundraTiles;
        private readonly TileSeries _mountainTiles;
        private readonly TileSeries _desertTiles;
        private readonly TileSeries _forestTiles;
        private readonly ITile _characterTile;

        private readonly byte[,] _map;

        public GameScene()
        {
            using ( Stream stream = GetType().Assembly.GetManifestResourceStream( "Resourcer.Resources.Embedded.overworld.png" ) )
            {
                _overworld = new TileSet( stream, 16 );
            }

            _grassTiles = new TileSeries(
                _overworld.TileAt( 0, 0 ),
                _overworld.TileAt( 7, 9 ),
                _overworld.TileAt( 8, 9 ),
                _overworld.TileAt( 7, 10 ),
                _overworld.TileAt( 8, 10 ) );

            _waterTiles = new TileSeries(
                _overworld.TileAt( 0, 1 ),
                _overworld.TileAt( 1, 1 ),
                _overworld.TileAt( 2, 1 ),
                _overworld.TileAt( 3, 1 ),
                _overworld.TileAt( 0, 2 ),
                _overworld.TileAt( 1, 2 ),
                _overworld.TileAt( 2, 2 ),
                _overworld.TileAt( 3, 2 ) );

            _tundraTiles = new TileSeries(
                _overworld.TileAt( 14, 11 ),
                _overworld.TileAt( 14, 12 ) );

            _mountainTiles = new TileSeries(
                new AdditiveTile( _overworld.TileAt( 0, 0 ), _overworld.TileAt( 7, 5 ) ) );

            _desertTiles = new TileSeries( _overworld.TileAt( 2, 32 ) );

            _forestTiles = new TileSeries(
                new AdditiveTile( _overworld.TileAt( 0, 0 ), _overworld.TileAt( 2, 14 ) ) );

            using ( Stream stream = GetType().Assembly.GetManifestResourceStream( "Resourcer.Resources.Embedded.m_01.png" ) )
            {
                var character = new TileSet( stream, 16, 1 );

                _characterTile = new AnimatedTile(
                    character.TileAt( 0, 0 ),
                    character.TileAt( 0, 1 ),
                    character.TileAt( 0, 2 ) );
            }

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<INoiseGenerator2D>( new PerlinNoise( 1234u ) );
            serviceCollection.AddSingleton<IParameterGenerator2D, ParameterGenerator2D>();
            serviceCollection.AddSingleton<IMapParameterGenerator2D, MapParameterGenerator2D>();
            serviceCollection.AddSingleton<IMapCellProvider<byte>, TerrainCellProvider>();
            serviceCollection.AddSingleton( typeof( IMapGenerator2D<> ), typeof( MapGenerator2D<> ) );
            var serviceProvider = serviceCollection.BuildServiceProvider();
            
            using var scope = serviceProvider.CreateScope();
            var mg = scope.ServiceProvider.GetRequiredService<IMapGenerator2D<byte>>();

            var sw = System.Diagnostics.Stopwatch.StartNew();
            _map = mg.CreateMap( 0, 0, 256, 256 );
            sw.Stop();

            sw = System.Diagnostics.Stopwatch.StartNew();
            _map = mg.CreateMap( 0, 0, 256, 256 );
            sw.Stop();
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
                        var destination = new SKRect( x, y, x + 64, y + 64 );

                        var biome = _map[mapX, mapY];
                        if ( biome == 'G' )
                        {
                            var tileX = Math.Abs( mapX ) % _grassTiles.Count;
                            _grassTiles.DrawTile( tileX, canvas, destination );
                        }
                        else if ( biome == 'W' )
                        {
                            _waterTiles.DrawTile( 0, canvas, destination );
                        }
                        else if ( biome == 'T' )
                        {
                            _tundraTiles.DrawTile( 0, canvas, destination );
                        }
                        else if ( biome == 'M' )
                        {
                            _mountainTiles.DrawTile( 0, canvas, destination );
                        }
                        else if ( biome == 'D' )
                        {
                            _desertTiles.DrawTile( 0, canvas, destination );
                        }
                        else if ( biome == 'F' )
                        {
                            _forestTiles.DrawTile( 0, canvas, destination );
                        }
                        else
                        {

                        }

                        if ( mapX == 10 && mapY == 10 )
                        {
                            _characterTile.Draw( canvas, destination );
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
