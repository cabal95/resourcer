using OpenGameKit.Abstractions;

namespace OpenGameKit.Generators;

/// <summary>
/// Used by generators to provide multiple characteristics to the
/// generated values.
/// </summary>
public class ParameterWave : IParameterWave
{
    /// <summary>
    /// A fractional seed value that is used to ensure this save generates
    /// different values than other waves being used.
    /// </summary>
    public float Seed { get; }

    /// <summary>
    /// The frequency of the peaks and dips in the wave as compared to other
    /// waves. A higher value means wider waves, a smaller value means narrower
    /// waves.
    /// </summary>
    public float Frequency { get; }

    /// <summary>
    /// The amplitude of this wave. Essentially, this dictates how strong this
    /// wave's values are in comparison to other waves.
    /// </summary>
    public float Amplitude { get; }

    /// <summary>
    /// Creates a new instance of <see cref="ParameterWave"/>.
    /// </summary>
    /// <param name="seed">The seed value for this wave to ensure unique values.</param>
    /// <param name="frequency">The frequency of the wave.</param>
    /// <param name="amplitude">The amplitude of the wave.</param>
    public ParameterWave( float seed, float frequency, float amplitude )
    {
        Seed = seed;
        Frequency = frequency;
        Amplitude = amplitude;
    }
}
