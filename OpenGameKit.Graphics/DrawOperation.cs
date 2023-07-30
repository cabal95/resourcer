using SkiaSharp;

namespace OpenGameKit.Graphics;

/// <summary>
/// The default implementation for a draw operation.
/// </summary>
public class DrawOperation : IDrawOperation
{
    /// <summary>
    /// The canvas object that will be returned in GetService().
    /// </summary>
    private readonly SKCanvas _canvas;

    /// <summary>
    /// The service provider that will be used in GetService().
    /// </summary>
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Creates a new instance of <see cref="DrawOperation"/>.
    /// </summary>
    /// <param name="canvas">The canvas the operation will draw to.</param>
    /// <param name="engineServices">The services available to the operation.</param>
    public DrawOperation( SKCanvas canvas, IServiceProvider engineServices )
    {
        _canvas = canvas;
        _serviceProvider = engineServices;
    }

    /// <inheritdoc/>
    public object? GetService( Type serviceType )
    {
        if ( serviceType == typeof( SKCanvas ) )
        {
            return _canvas;
        }

        return _serviceProvider.GetService( serviceType );
    }
}
