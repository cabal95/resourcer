using OpenGameKit.Abstractions;

using System.Numerics;

namespace OpenGameKit.Generators;

/// <summary>
/// A 2D noise generator implementation based on the Perlin noise pattern.
/// </summary>
public class PerlinNoise : INoiseGenerator2D
{
    /// <summary>
    /// The seed value this generator was initialized with.
    /// </summary>
    private readonly uint _seed;

    /// <summary>
    /// Creates a new instance of <see cref="PerlinNoise"/>.
    /// </summary>
    /// <param name="seed">The seed value to use for random data. Multiple instances using the same seed will produce the same values for the specified positions.</param>
    public PerlinNoise( uint seed )
    {
        _seed = seed;
    }

    /// <summary>
    /// Function to linearly interpolate between a0 and a1.
    /// </summary>
    /// <param name="lower">The lower value.</param>
    /// <param name="upper">The upper value.</param>
    /// <param name="weight">The weighted value in the range [0.0, 1.0].</param>
    /// <returns>A new value.</returns>
    private static float Interpolate( float lower, float upper, float weight )
    {
        return lower + ( upper - lower ) * ( weight * weight * weight * ( weight * ( weight * 6 - 15 ) + 10 ) );
    }

    /// <summary>
    /// Create pseudorandom direction vector.
    /// </summary>
    /// <param name="ix"></param>
    /// <param name="iy"></param>
    /// <returns></returns>
    private Vector2 RandomGradient( int ix, int iy )
    {
        int w = 8 * sizeof( int );
        int s = w / 2; // rotation width
        uint a = ( uint ) ix;
        uint b = ( uint ) iy;

        a *= 3284157443;
        b ^= a << s | a >> w - s;
        b *= 1911520717;
        a ^= b << s | b >> w - s;
        a *= 2048419325;

        a *= _seed;

        float random = ( a / ( float ) uint.MaxValue ) * ( float ) Math.PI * 2.0f;

        return new Vector2( ( float ) Math.Cos( random ), ( float ) Math.Sin( random ) );
    }

    /// <summary>
    /// Computes the dot product of the distance and gradient vectors.
    /// </summary>
    /// <param name="ix"></param>
    /// <param name="iy"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private float DotGridGradient( int ix, int iy, float x, float y )
    {
        var gradient = RandomGradient( ix, iy );

        float dx = x - ( float ) ix;
        float dy = y - ( float ) iy;

        var value = ( dx * gradient.X + dy * gradient.Y );

        return value;
    }

    /// <inheritdoc/>
    public float Noise( float x, float y )
    {
        // Determine grid cell coordinates
        int x0 = ( int ) Math.Floor( x );
        int x1 = x0 + 1;
        int y0 = ( int ) Math.Floor( y );
        int y1 = y0 + 1;

        // Determine interpolation weights
        // Could also use higher order polynomial/s-curve here
        float sx = x - ( float ) x0;
        float sy = y - ( float ) y0;

        // Interpolate between grid point gradients
        float n0, n1, ix0, ix1, value;

        n0 = DotGridGradient( x0, y0, x, y );
        n1 = DotGridGradient( x1, y0, x, y );
        ix0 = Interpolate( n0, n1, sx );

        n0 = DotGridGradient( x0, y1, x, y );
        n1 = DotGridGradient( x1, y1, x, y );
        ix1 = Interpolate( n0, n1, sx );

        value = Interpolate( ix0, ix1, sy );
        value = ( value * 0.5f ) + 0.5f;

        return Normalize( value );
    }

    /// <summary>
    /// Normalize the value. Normally these values land between 0.25 and 0.75
    /// so we stretch them out to be between 0 and 1.
    /// </summary>
    /// <param name="value">The value to be normalized.</param>
    /// <returns>The normalized value.</returns>
    private static float Normalize( float value )
    {
        float minValue = 0.25f;
        float maxValue = 0.75f;

        return Math.Clamp( ( value - minValue ) / ( maxValue - minValue ), 0f, 1f );
    }
}
