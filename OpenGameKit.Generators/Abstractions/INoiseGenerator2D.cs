namespace OpenGameKit.Generators.Abstractions;

/// <summary>
/// Defines a two dimensional noise generator.
/// </summary>
public interface INoiseGenerator2D
{
    /// <summary>
    /// Generates a noise value for the given position.
    /// </summary>
    /// <param name="x">The x coordinate, fractional numbers should be used.</param>
    /// <param name="y">The y coordinate, fractional numbers should be used.</param>
    /// <returns>The noise value for the given coordinates, between 0 and 1 inclusive.</returns>
    float Noise( float x, float y );
}
