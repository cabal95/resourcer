using OpenGameKit.Abstractions;

namespace OpenGameKit.Graphics;

/// <summary>
/// The default implementation for a draw operation.
/// </summary>
public class DrawOperation : IDrawOperation
{
    /// <summary>
    /// The canvas object that drawing operations should be performed on.
    /// </summary>
    public ICanvas Canvas { get; }

    /// <summary>
    /// The service provider that will be used in GetService().
    /// </summary>
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Creates a new instance of <see cref="DrawOperation"/>.
    /// </summary>
    /// <param name="canvas">The canvas the operation will draw to.</param>
    /// <param name="engineServices">The services available to the operation.</param>
    public DrawOperation( ICanvas canvas, IServiceProvider engineServices )
    {
        Canvas = canvas;
        _serviceProvider = engineServices;
    }

    /// <inheritdoc/>
    public object? GetService( Type serviceType )
    {
        return _serviceProvider.GetService( serviceType );
    }
}
