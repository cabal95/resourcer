using OpenGameKit.Abstractions;

namespace OpenGameKit.Generators;

/// <summary>
/// The default 2D map generator implementation.
/// </summary>
public class MapParameterGenerator2D : IMapParameterGenerator2D
{
    /// <summary>
    /// The parameter generator to use when creating the map cell data.
    /// </summary>
    private readonly IParameterGenerator2D _parameterGenerator;

    /// <summary>
    /// Creates a new instance of <see cref="MapParameterGenerator2D"/>.
    /// </summary>
    /// <param name="parameterGenerator">The parameter generator for map cell data.</param>
    public MapParameterGenerator2D( IParameterGenerator2D parameterGenerator )
    {
        _parameterGenerator = parameterGenerator;
    }

    /// <inheritdoc/>
    public IReadOnlyDictionary<string, float>[,] Generate( int x, int y, uint width, uint height, IReadOnlyList<IParameterDescription> parameters )
    {
        var map = new Dictionary<string, float>[width, height];

        for ( int yIndex = 0; yIndex < height; yIndex++ )
        {
            for ( int xIndex = 0; xIndex < width; xIndex++ )
            {
                var waveMap = new Dictionary<string, float>();

                for ( int w = 0; w < parameters.Count; w++ )
                {
                    var value = _parameterGenerator.Generate( x + xIndex, y + yIndex, parameters[w].Waves );

                    waveMap.Add( parameters[w].Name, value );
                }

                map[xIndex, yIndex] = waveMap;
            }
        }

        return map;
    }
}
