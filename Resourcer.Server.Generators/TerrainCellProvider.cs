using OpenGameKit.Abstractions;
using OpenGameKit.Generators;

namespace Resourcer.Server.Generators;

/// <summary>
/// The cell provider for our standard map terrain.
/// </summary>
public class TerrainCellProvider : IMapCellProvider<byte>
{
    #region Constants

    /// <summary>
    /// The biomes that will be used during construction of the map.
    /// </summary>
    private static readonly BiomeDefinition[] Biomes = new BiomeDefinition[]
    {
        new BiomeDefinition( 'W', 0.0f, 0.0f, 0.0f, 0.5f ), // Water/Ocean
        new BiomeDefinition( 'G', 0.2f, 0.3f, 0.2f ), // Grassland
        new BiomeDefinition( 'F', 0.35f, 0.6f, 0.5f ), // Forest
        new BiomeDefinition( 'D', 0.2f, 0.0f, 0.75f, 1.0f, 0.5f, 1.0f ), // Desert
        new BiomeDefinition( 'M', 0.65f, 0.3f, 0.0f ), // Mountains
        new BiomeDefinition( 'T', 0.5f, 0.4f, 0.0f, 1.0f, 1.0f, 0.25f ) // Tundra
    };

    /// <summary>
    /// The parameters that will be used to describe each cell of the map.
    /// </summary>
    private static readonly IParameterDescription[] Parameters = new [] 
    {
        new ParameterDescription( "height", new []
        {
            new ParameterWave( 56.0f, 0.05f, 1.0f ),
            new ParameterWave( 199.36f, 0.1f, 0.5f )
        } ),
        new ParameterDescription( "moisture", new []
        {
            new ParameterWave( 621.0f, 0.03f, 1.0f ),
            new ParameterWave( 329.7f, 0.02f, 0.05f )
        } ),
        new ParameterDescription( "heat", new []
        {
            new ParameterWave( 318.6f, 0.04f, 1.0f ),
            new ParameterWave( 329.7f, 0.02f, 0.05f )
        } )
    };

    #endregion

    /// <inheritdoc/>
    public byte CreateCell( IReadOnlyDictionary<string, float> parameters )
    {
        var height = parameters["height"];
        var moisture = parameters["moisture"];
        var heat = parameters["heat"];

        var bestBiome = Biomes
            .Where( b => b.Matches( height, moisture, heat ) )
            .OrderBy( b => b.GetDiffValue( height, moisture, heat ) )
            .FirstOrDefault();

        return ( byte ) ( bestBiome?.Id ?? Biomes[0].Id );
    }

    /// <inheritdoc/>
    public IReadOnlyList<IParameterDescription> GetParameters()
    {
        return Parameters;
    }
}
