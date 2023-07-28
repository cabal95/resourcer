using Microsoft.Extensions.DependencyInjection;

using SkiaSharp;

namespace Resourcer.UI;

public class SceneManager
{
    private readonly IPlatform _platform;

    private IScene _currentScene;

    private readonly IServiceProvider _serviceProvider;

    public SceneManager( IServiceProvider serviceProvider, IPlatform platform )
    {
        _serviceProvider = serviceProvider;
        _platform = platform;

        _currentScene = ActivatorUtilities.GetServiceOrCreateInstance<GameScene>( _serviceProvider );

        Task.Run( async () =>
        {
            while ( true )
            {
                await Task.Delay( ( int ) ( 1_000 / 60.0 ) );
                
                _platform.UpdateCanvas();
            }
        } );
    }

    public void Paint( SKCanvas canvas, SKSizeI size, SKRectI dirtyRect )
    {
        _currentScene.Draw( canvas, size, dirtyRect );
    }
}
