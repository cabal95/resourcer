using System.Numerics;

namespace MapGenerator;

public class BiomeGenerator
{
    public int Width { get; set; } = 100;

    public int Height { get; set; } = 100;

    public float Scale { get; set; } = 1f;

    public Vector2 Offset { get; set; } = new Vector2( 0, 0 );

    public Wave[] HeightWaves { get; set; } = new Wave[]
    {
        new Wave { Seed = 56.0f, Frequency = 0.05f, Amplitude = 1.0f },
        new Wave { Seed = 199.36f, Frequency = 0.1f, Amplitude = 0.5f }
    };

    public Wave[] MoistureWaves { get; set; } = new Wave[]
    {
        new Wave { Seed = 621.0f, Frequency = 0.03f, Amplitude = 1.0f },
        new Wave { Seed = 329.7f, Frequency = 0.02f, Amplitude = 0.05f }
    };

    public Wave[] HeatWaves { get; set; } = new Wave[]
    {
        new Wave { Seed = 318.6f, Frequency = 0.04f, Amplitude = 1.0f },
        new Wave { Seed = 329.7f, Frequency = 0.02f, Amplitude = 0.05f }
    };

    private float[,] _heightMap = new float[0, 0];

    private float[,] _moistureMap = new float[0, 0];

    private float[,] _heatMap = new float[0, 0];

    public static float[,] Generate( int width, int height, float scale, Wave[] waves, Vector2 offset, uint seed )
    {
        //var qn = QuickNoise.Create();
        var pn = new PerlinNoise( seed );

        // create the noise map
        float[,] noiseMap = new float[width, height];
        // loop through each element in the noise map
        for ( int x = 0; x < width; ++x )
        {
            for ( int y = 0; y < height; ++y )
            {
                // calculate the sample positions
                float samplePosX = ( x + offset.X ) * scale;
                float samplePosY = ( y + offset.Y ) * scale;
                float normalization = 0.0f;
                // loop through each wave
                foreach ( Wave wave in waves )
                {
                    // sample the perlin noise taking into consideration amplitude and frequency
                    noiseMap[x, y] += wave.Amplitude * pn.Noise( samplePosX * wave.Frequency + wave.Seed, samplePosY * wave.Frequency + wave.Seed );
                    //noiseMap[x, y] += wave.Amplitude * ((qn.Noise( samplePosX * wave.Frequency + wave.Seed, samplePosY * wave.Frequency + wave.Seed, 1 ) * 0.5f) + 0.5f);
                    normalization += wave.Amplitude;
                }
                // normalize the value
                noiseMap[x, y] /= normalization;
            }
        }

        NormalizeMap( width, height, noiseMap );

        return noiseMap;
    }

    private static void NormalizeMap( int width, int height, float[,] map )
    {
        float minValue = 0.25f;
        float maxValue = 0.75f;

        if ( maxValue > minValue )
        {
            for ( int x = 0; x < width; x++ )
            {
                for ( int y = 0; y < height; y++ )
                {
                    map[x, y] = Math.Clamp( ( map[x, y] - minValue ) / ( maxValue - minValue ), 0f, 1f );
                }
            }
        }
    }

    public static BiomePreset[] GetDefaultBiomes()
    {
        return new BiomePreset[]
        {
            new BiomePreset { Id = 'W', MinHeight = 0.0f, MinMoisture = 0.0f, MinHeat = 0.0f }, // Water/Ocean
            new BiomePreset { Id = 'G', MinHeight = 0.2f, MinMoisture = 0.3f, MinHeat = 0.2f }, // Grassland
            new BiomePreset { Id = 'F', MinHeight = 0.35f, MinMoisture = 0.6f, MinHeat = 0.5f }, // Forest
            new BiomePreset { Id = 'D', MinHeight = 0.2f, MinMoisture = 0.0f, MinHeat = 0.75f }, // Desert
            new BiomePreset { Id = 'M', MinHeight = 0.65f, MinMoisture = 0.3f, MinHeat = 0.0f }, // Mountains
            new BiomePreset { Id = 'T', MinHeight = 0.5f, MinMoisture = 0.2f, MinHeat = 0.0f } // Tundra
        };
    }

    public byte[,] GenerateMap( uint seed )
    {
        _heightMap = Generate( Width, Height, Scale, HeightWaves, Offset, seed );

        _moistureMap = Generate( Width, Height, Scale, MoistureWaves, Offset, seed );

        _heatMap = Generate( Width, Height, Scale, HeatWaves, Offset, seed );

        var dist = new int[10];

        for ( int x = 0; x < Width; x++ )
        {
            for ( int y = 0; y < Height; y++ )
            {
                for ( int z = 1; z <= 10; z++ )
                {
                    if ( _moistureMap[x, y] <= ( 0.1f * z ) )
                    {
                        dist[z - 1]++;
                        break;
                    }
                }
            }
        }

        var map = new byte[Width, Height];

        for ( int x = 0; x < Width; ++x )
        {
            for ( int y = 0; y < Height; ++y )
            {
                map[x, y] = ( byte ) GetBiome( _heightMap[x, y], _moistureMap[x, y], _heatMap[x, y] ).Id;
            }
        }

        return map;
    }

    private static BiomePreset GetBiome( float height, float moisture, float heat )
    {
        if ( height > 1 || moisture > 1 || heat > 1 || height < 0.3 || moisture < 0.3 || heat < 0.3 )
        {
        }
        var biomeTemp = new List<BiomeTempData>();

        foreach ( BiomePreset biome in GetDefaultBiomes() )
        {
            if ( biome.Matches( height, moisture, heat ) )
            {
                biomeTemp.Add( new BiomeTempData( biome ) );
            }
        }

        float curVal = 0.0f;
        BiomePreset? BiomeToReturn = null;

        foreach ( BiomeTempData biome in biomeTemp )
        {
            if ( BiomeToReturn == null )
            {
                BiomeToReturn = biome.Biome;
                curVal = biome.GetDiffValue( height, moisture, heat );
            }
            else
            {
                if ( biome.GetDiffValue( height, moisture, heat ) < curVal )
                {
                    BiomeToReturn = biome.Biome;
                    curVal = biome.GetDiffValue( height, moisture, heat );
                }
            }
        }

        if ( BiomeToReturn == null )
        {
            BiomeToReturn = GetDefaultBiomes()[0];
        }

        return BiomeToReturn;
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

public class Wave
{
    public float Seed { get; set; }

    public float Frequency { get; set; }

    public float Amplitude { get; set; }
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