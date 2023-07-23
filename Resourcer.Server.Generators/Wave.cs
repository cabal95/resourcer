namespace Resourcer.Server.Generators;

/// <summary>
/// Used by generators to provide multiple characteristics to the
/// generated values.
/// </summary>
public class Wave
{
    /// <summary>
    /// A fractional seed value that is used to ensure this save generates
    /// different values than other waves being used.
    /// </summary>
    public float Seed { get; set; }

    /// <summary>
    /// The frequency of the peaks and dips in the wave as compared to other
    /// waves. A higher value means wider waves, a smaller value means narrower
    /// waves.
    /// </summary>
    public float Frequency { get; set; } = 1.0f;

    /// <summary>
    /// The amplitude of this wave. Essentially, this dictates how strong this
    /// wave's values are in comparison to other waves.
    /// </summary>
    public float Amplitude { get; set; } = 1.0f;
}
