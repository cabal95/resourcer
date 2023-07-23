using OpenGameKit.Generators.Abstractions;

namespace OpenGameKit.Generators;

/// <summary>
/// The default 2D map generator implementation.
/// </summary>
/// <typeparam name="TCell">The type of object to use when generating the map cells.</typeparam>
public class MapGenerator2D<TCell> : IMapGenerator2D<TCell>
{
    /// <summary>
    /// The object that will provide the cell objects and the parameter descriptions.
    /// </summary>
    private readonly IMapCellProvider<TCell> _cellProvider;

    /// <summary>
    /// The parameter generator that will be used to create the cell data.
    /// </summary>
    private readonly IMapParameterGenerator2D _mapParameterGenerator;

    /// <summary>
    /// Creates a new instance of <see cref="MapGenerator2D{TCell}"/>.
    /// </summary>
    /// <param name="cellProvider">The object that will provide the cell objects and the parameter descriptions.</param>
    /// <param name="mapParameterGenerator">The object that will generate the random parameter values.</param>
    public MapGenerator2D( IMapCellProvider<TCell> cellProvider, IMapParameterGenerator2D mapParameterGenerator )
    {
        _cellProvider = cellProvider;
        _mapParameterGenerator = mapParameterGenerator;
    }

    /// <inheritdoc/>
    public TCell[,] CreateMap( int x, int y, uint width, uint height )
    {
        var map = new TCell[width, height];
        var mapParameters = _mapParameterGenerator.Generate( x, y, width, height, _cellProvider.GetParameters() );

        for (int yIndex = 0; yIndex < height; yIndex++ )
        {
            for (int xIndex = 0;  xIndex < width; xIndex++ )
            {
                map[xIndex, yIndex] = _cellProvider.CreateCell( mapParameters[xIndex, yIndex] );
            }
        }

        return map;
    }
}
