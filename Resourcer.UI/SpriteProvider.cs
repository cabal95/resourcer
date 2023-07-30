using Microsoft.Extensions.DependencyInjection;

using OpenGameKit.Abstractions;
using OpenGameKit.Generators;
using OpenGameKit.Generators.Abstractions;
using OpenGameKit.Graphics;

using Resourcer.Server.Generators;

namespace Resourcer.UI;

/// <summary>
/// Provides all the sprites for the UI.
/// </summary>
public class SpriteProvider
{
    /// <summary>
    /// The set of tiles that will be used to draw grass.
    /// </summary>
    public IReadOnlyList<ISprite> GrassTiles { get; }

    /// <summary>
    /// The set of tiles that will be used to draw water.
    /// </summary>
    public IReadOnlyList<ISprite> WaterTiles { get; }

    /// <summary>
    /// The set of tiles that will be used to draw tundra.
    /// </summary>
    public IReadOnlyList<ISprite> TundraTiles { get; }

    /// <summary>
    /// The set of tiles that will be used to draw mountain.
    /// </summary>
    public IReadOnlyList<ISprite> MountainTiles { get; }

    /// <summary>
    /// The set of tiles that will be used to draw desert.
    /// </summary>
    public IReadOnlyList<ISprite> DesertTiles { get; }

    /// <summary>
    /// the set of tiles that will be used to draw forest.
    /// </summary>
    public IReadOnlyList<ISprite> ForestTiles { get; }

    /// <summary>
    /// The sprite that will be used to draw the character.
    /// </summary>
    public ISprite CharacterTile { get; }

    public readonly byte[,] Map;

    /// <summary>
    /// Creates a new instance of <see cref="SpriteProvider"/>.
    /// </summary>
    public SpriteProvider()
    {
        using ( var stream = GetType().Assembly.GetManifestResourceStream( "Resourcer.UI.Resources.Embedded.overworld.png" ) )
        {
            if ( stream == null )
            {
                throw new Exception( "World tile set not found." );
            }

            var overworld = new GridTileSet( stream, 16, 16 );

            GrassTiles = new[]
            {
                overworld.GetTileAt( 0, 0 ),
                overworld.GetTileAt( 7, 9 ),
                overworld.GetTileAt( 8, 9 ),
                overworld.GetTileAt( 7, 10 ),
                overworld.GetTileAt( 8, 10 )
            };

            WaterTiles = new[]
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

            TundraTiles = new[]
            {
                overworld.GetTileAt( 14, 11 ),
                overworld.GetTileAt( 14, 12 )
            };

            MountainTiles = new[]
            {
                new LayeredSprite( overworld.GetTileAt( 0, 0 ), overworld.GetTileAt( 7, 5 ) )
            };

            DesertTiles = new[] { overworld.GetTileAt( 2, 32 ) };

            ForestTiles = new[]
            {
                new LayeredSprite( overworld.GetTileAt( 0, 0 ), overworld.GetTileAt( 2, 14 ) )
            };
        }

        using ( var stream = GetType().Assembly.GetManifestResourceStream( "Resourcer.UI.Resources.Embedded.m_01.png" ) )
        {
            if ( stream == null )
            {
                throw new Exception( "Character tile set not found." );
            }

            var character = new UnstructuredTileSet( stream );

            CharacterTile = new AnimatedSprite(
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
        Map = mg.CreateMap( 0, 0, 256, 256 );
        sw.Stop();

        sw = System.Diagnostics.Stopwatch.StartNew();
        Map = mg.CreateMap( 0, 0, 256, 256 );
        sw.Stop();

        System.Diagnostics.Debug.WriteLine( $"Created 256x256 map chunk in {sw.Elapsed.TotalMilliseconds}ms." );
    }
}
