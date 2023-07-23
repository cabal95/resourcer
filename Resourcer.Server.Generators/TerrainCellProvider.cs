using OpenGameKit.Generators;
using OpenGameKit.Generators.Abstractions;

namespace Resourcer.Server.Generators;

public class TerrainCellProvider : IMapCellProvider<byte>
{
    private static readonly BiomePreset[] _biomes = new BiomePreset[]
    {
        new BiomePreset { Id = 'W', MinHeight = 0.0f, MinMoisture = 0.0f, MinHeat = 0.0f }, // Water/Ocean
        new BiomePreset { Id = 'G', MinHeight = 0.2f, MinMoisture = 0.3f, MinHeat = 0.2f }, // Grassland
        new BiomePreset { Id = 'F', MinHeight = 0.35f, MinMoisture = 0.6f, MinHeat = 0.5f }, // Forest
        new BiomePreset { Id = 'D', MinHeight = 0.2f, MinMoisture = 0.0f, MinHeat = 0.75f }, // Desert
        new BiomePreset { Id = 'M', MinHeight = 0.65f, MinMoisture = 0.3f, MinHeat = 0.0f }, // Mountains
        new BiomePreset { Id = 'T', MinHeight = 0.5f, MinMoisture = 0.2f, MinHeat = 0.0f } // Tundra
    };

    private static readonly IParameterDescription[] _parameters = new [] 
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

    /// <inheritdoc/>
    public byte CreateCell( IReadOnlyDictionary<string, float> parameters )
    {
        var height = parameters["height"];
        var moisture = parameters["moisture"];
        var heat = parameters["heat"];

        var bestBiome = _biomes
            .Where( b => b.Matches( height, moisture, heat ) )
            .OrderBy( b => b.GetDiffValue( height, moisture, heat ) )
            .FirstOrDefault();

        return ( byte ) ( bestBiome?.Id ?? _biomes[0].Id );
    }

    public IReadOnlyList<IParameterDescription> GetParameters()
    {
        return _parameters;
    }
}
