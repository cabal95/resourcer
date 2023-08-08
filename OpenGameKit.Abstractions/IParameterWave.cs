namespace OpenGameKit.Abstractions;

/// <summary>
/// Used by generators to provide multiple characteristics to the
/// generated values.
/// </summary>
public interface IParameterWave
{
    /// <summary>
    /// A fractional seed value that is used to ensure this save generates
    /// different values than other waves being used.
    /// </summary>
    float Seed { get; }

    /// <summary>
    /// The frequency of the peaks and dips in the wave as compared to other
    /// waves. A higher value means wider waves, a smaller value means narrower
    /// waves.
    /// </summary>
    float Frequency { get; }

    /// <summary>
    /// The amplitude of this wave. Essentially, this dictates how strong this
    /// wave's values are in comparison to other waves.
    /// </summary>
    float Amplitude { get; }
}
