using Microsoft.Extensions.DependencyInjection;

using OpenGameKit.Graphics;

using SkiaSharp;

namespace Resourcer.UI;

public class SceneManager
{
    private readonly IPlatform _platform;

    private readonly IScene _currentScene;

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
        if ( _currentScene.Width != size.Width || _currentScene.Height != size.Height )
        {
            _currentScene.SetSize( size.Width, size.Height );
        }

        _currentScene.Draw( canvas, dirtyRect );
    }

    public void Pan( SKPointI amount )
    {
        if ( _currentScene is GameScene gameScene )
        {
            gameScene.Offset = new SKPointI( gameScene.Offset.X - amount.X, gameScene.Offset.Y - amount.Y );
            _platform.UpdateCanvas();
        }
    }
}
