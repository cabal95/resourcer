using System.IO;

using Microsoft.Extensions.DependencyInjection;

using OpenGameKit.Generators;
using OpenGameKit.Generators.Abstractions;
using OpenGameKit.Graphics;

using Resourcer.Server.Generators;

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

        private readonly IReadOnlyList<ISprite> _grassTiles;
        private readonly IReadOnlyList<ISprite> _waterTiles;
        private readonly IReadOnlyList<ISprite> _tundraTiles;
        private readonly IReadOnlyList<ISprite> _mountainTiles;
        private readonly IReadOnlyList<ISprite> _desertTiles;
        private readonly IReadOnlyList<ISprite> _forestTiles;
        private readonly ISprite _characterTile;

        private readonly byte[,] _map;

        public GameScene()
        {
            using ( Stream stream = GetType().Assembly.GetManifestResourceStream( "Resourcer.Resources.Embedded.overworld.png" ) )
            {
                var overworld = new GridTileSet( stream, 16, 16 );

                _grassTiles = new[]
                {
                    overworld.GetTileAt( 0, 0 ),
                    overworld.GetTileAt( 7, 9 ),
                    overworld.GetTileAt( 8, 9 ),
                    overworld.GetTileAt( 7, 10 ),
                    overworld.GetTileAt( 8, 10 )
                };

                _waterTiles = new[]
                {
                    overworld.GetTileAt( 0, 1 ),
                    overworld.GetTileAt( 1, 1 ),
                    overworld.GetTileAt( 2, 1 ),
                    overworld.GetTileAt( 3, 1 ),
                    overworld.GetTileAt( 0, 2 ),
                    overworld.GetTileAt( 1, 2 ),
                    overworld.GetTileAt( 2, 2 ),
                    overworld.GetTileAt( 3, 2 )
                };

                _tundraTiles = new[]
                {
                    overworld.GetTileAt( 14, 11 ),
                    overworld.GetTileAt( 14, 12 )
                };

                _mountainTiles = new[]
                {
                    new LayeredSprite( overworld.GetTileAt( 0, 0 ), overworld.GetTileAt( 7, 5 ) )
                };

                _desertTiles = new[] { overworld.GetTileAt( 2, 32 ) };

                _forestTiles = new[]
                {
                    new LayeredSprite( overworld.GetTileAt( 0, 0 ), overworld.GetTileAt( 2, 14 ) )
                };
            }

            using ( Stream stream = GetType().Assembly.GetManifestResourceStream( "Resourcer.Resources.Embedded.m_01.png" ) )
            {
                var character = new UnstructuredTileSet( stream );

                _characterTile = new AnimatedSprite(
                    character.GetTileAt( 0, 1, 16, 16 ),
                    character.GetTileAt( 0, 18, 16, 16 ),
                    character.GetTileAt( 0, 1, 16, 16 ),
                    character.GetTileAt( 0, 35, 16, 16 ) );
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

            System.Diagnostics.Debug.WriteLine( $"Created 256x256 map chunk in {sw.Elapsed.TotalMilliseconds}ms." );
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

                        var biome = _map[mapX, mapY];
                        if ( biome == 'G' )
                        {
                            var tileX = Math.Abs( mapX ) % _grassTiles.Count;
                            _grassTiles[tileX].Draw( canvas, destination );
                        }
                        else if ( biome == 'W' )
                        {
                            _waterTiles[0].Draw( canvas, destination );
                        }
                        else if ( biome == 'T' )
                        {
                            _tundraTiles[0].Draw( canvas, destination );
                        }
                        else if ( biome == 'M' )
                        {
                            _mountainTiles[0].Draw( canvas, destination );
                        }
                        else if ( biome == 'D' )
                        {
                            _desertTiles[0].Draw( canvas, destination );
                        }
                        else if ( biome == 'F' )
                        {
                            _forestTiles[0].Draw( canvas, destination );
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
