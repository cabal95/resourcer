namespace Resourcer.Server.Generators;

public interface IParameterGenerator2D
{
    float Generate( float x, float y, Wave[] waves );
}

public class ParameterGenerator2D : IParameterGenerator2D
{
    private readonly INoiseGenerator2D _noiseGenerator;

    public ParameterGenerator2D( INoiseGenerator2D noiseGenerator )
    {
        _noiseGenerator = noiseGenerator;
    }

    public float Generate( float x, float y, Wave[] waves )
    {
        var normalization = 0.0f;
        var value = 0.0f;

        for ( int w = 0; w < waves.Length; w++ )
        {
            var wave = waves[w];

            value += wave.Amplitude * _noiseGenerator.Noise( ( x * wave.Frequency ) + wave.Seed, ( y * wave.Frequency ) + wave.Seed );
            normalization += wave.Amplitude;
        }

        return value / normalization;
    }
}

public interface IMapParameterGenerator2D
{
    IReadOnlyDictionary<string, float>[,] Generate( int x, int y, uint width, uint height, Wave[][] parameterWaves );
}

public class MapParameterGenerator2D : IMapParameterGenerator2D
{
    private readonly IParameterGenerator2D _parameterGenerator;

    public MapParameterGenerator2D( IParameterGenerator2D parameterGenerator )
    {
        _parameterGenerator = parameterGenerator;
    }

    public IReadOnlyDictionary<string, float>[,] Generate( int x, int y, uint width, uint height, Wave[][] parameterWaves )
    {
        var map = new Dictionary<string, float>[width, height];

        for ( int yIndex = 0; yIndex < height; yIndex++ )
        {
            for ( int xIndex = 0; xIndex < width; xIndex++ )
            {
                var waveMap = new Dictionary<string, float>();

                for ( int w = 0; w < parameterWaves.Length; w++ )
                {
                    var value = _parameterGenerator.Generate( x + xIndex, y + yIndex, parameterWaves[w] );

                    waveMap.Add( w.ToString(), value );
                }

                map[xIndex, yIndex] = waveMap;
            }
        }

        return map;
    }
}

public interface IMapCellProvider<TCell>
{
    TCell CreateCell( IReadOnlyDictionary<string, float> parameters );

    Wave[][] GetParameterWaves();
}

public interface IMapGenerator2D<TCell>
{
    TCell[,] CreateMap( int x, int y, uint width, uint height );
}

public class MapGenerator2D<TCell> : IMapGenerator2D<TCell>
{
    private readonly IMapCellProvider<TCell> _cellGenerator;
    private readonly IMapParameterGenerator2D _mapParameterGenerator;

    public MapGenerator2D( IMapCellProvider<TCell> cellGenerator, IMapParameterGenerator2D mapParameterGenerator )
    {
        _cellGenerator = cellGenerator;
        _mapParameterGenerator = mapParameterGenerator;
    }

    public TCell[,] CreateMap( int x, int y, uint width, uint height )
    {
        var map = new TCell[width, height];
        var mapParameters = _mapParameterGenerator.Generate( x, y, width, height, _cellGenerator.GetParameterWaves() );

        for (int yIndex = 0; yIndex < height; yIndex++ )
        {
            for (int xIndex = 0;  xIndex < width; xIndex++ )
            {
                map[xIndex, yIndex] = _cellGenerator.CreateCell( mapParameters[xIndex, yIndex] );
            }
        }

        return map;
    }
}

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

    private static readonly Wave[][] _parameterWaves = new Wave[][]
    {
        // Height waves.
        new Wave[]
        {
                new Wave { Seed = 56.0f, Frequency = 0.05f, Amplitude = 1.0f },
                new Wave { Seed = 199.36f, Frequency = 0.1f, Amplitude = 0.5f }
        },

        // Moisture waves.
        new Wave[]
        {
                new Wave { Seed = 621.0f, Frequency = 0.03f, Amplitude = 1.0f },
                new Wave { Seed = 329.7f, Frequency = 0.02f, Amplitude = 0.05f }
        },

        // Heat waves.
        new Wave[]
        {
                new Wave { Seed = 318.6f, Frequency = 0.04f, Amplitude = 1.0f },
                new Wave { Seed = 329.7f, Frequency = 0.02f, Amplitude = 0.05f }
        }
    };

    public byte CreateCell( IReadOnlyDictionary<string, float> parameters )
    {
        var height = parameters["0"];
        var moisture = parameters["1"];
        var heat = parameters["2"];

        var biomeTemp = new List<BiomeTempData>();

        foreach ( BiomePreset biome in _biomes )
        {
            if ( biome.Matches( height, moisture, heat ) )
            {
                biomeTemp.Add( new BiomeTempData( biome ) );
            }
        }

        float curVal = 0.0f;
        BiomePreset? biomeToReturn = null;

        foreach ( BiomeTempData biome in biomeTemp )
        {
            if ( biomeToReturn == null )
            {
                biomeToReturn = biome.Biome;
                curVal = biome.GetDiffValue( height, moisture, heat );
            }
            else
            {
                if ( biome.GetDiffValue( height, moisture, heat ) < curVal )
                {
                    biomeToReturn = biome.Biome;
                    curVal = biome.GetDiffValue( height, moisture, heat );
                }
            }
        }

        biomeToReturn ??= _biomes[0];

        return ( byte ) biomeToReturn.Id;
    }

    public Wave[][] GetParameterWaves()
    {
        return _parameterWaves;
    }
}

public class BiomeTempData
{
    public BiomePreset Biome { get; }

    public BiomeTempData( BiomePreset preset )
    {
        Biome = preset;
    }

    public float GetDiffValue( float height, float moisture, float heat )
    {
        return ( height - Biome.MinHeight ) + ( moisture - Biome.MinMoisture ) + ( heat - Biome.MinHeat );
    }
}

public class BiomePreset
{
    public int Id { get; set; }

    public float MinHeight { get; set; }

    public float MinMoisture { get; set; }

    public float MinHeat { get; set; }

    public bool Matches( float height, float moisture, float heat )
    {
        return height >= MinHeight && moisture >= MinMoisture && heat >= MinHeat;
    }
}