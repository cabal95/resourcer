using OpenGameKit.Generators.Abstractions;

namespace OpenGameKit.Generators;

/// <summary>
/// The default parameter generator implementation.
/// </summary>
public class ParameterGenerator2D : IParameterGenerator2D
{
    /// <summary>
    /// The noise generator that will be used for generating the values.
    /// </summary>
    private readonly INoiseGenerator2D _noiseGenerator;

    /// <summary>
    /// Creates a new instance of <see cref="ParameterGenerator2D"/>.
    /// </summary>
    /// <param name="noiseGenerator">The noise generator that will be used to generate values.</param>
    public ParameterGenerator2D( INoiseGenerator2D noiseGenerator )
    {
        _noiseGenerator = noiseGenerator;
    }

    /// <inheritdoc/>
    public float Generate( int x, int y, IReadOnlyList<IParameterWave> waves )
    {
        var normalization = 0.0f;
        var value = 0.0f;

        for ( int w = 0; w < waves.Count; w++ )
        {
            var wave = waves[w];

            value += wave.Amplitude * _noiseGenerator.Noise( ( x * wave.Frequency ) + wave.Seed, ( y * wave.Frequency ) + wave.Seed );
            normalization += wave.Amplitude;
        }

        return value / normalization;
    }
}
