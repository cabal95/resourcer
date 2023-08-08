using Microsoft.Extensions.DependencyInjection;

using OpenGameKit.Graphics;

using SkiaSharp;

namespace Resourcer.UI;

public class SceneManager
{
    private readonly IPlatform _platform;

    private readonly IScene _currentScene;

    private readonly IServiceProvider _serviceProvider;

    private readonly Timer _timer;

    public SceneManager( IServiceProvider serviceProvider, IPlatform platform )
    {
        _serviceProvider = serviceProvider;
        _platform = platform;

        _currentScene = ActivatorUtilities.GetServiceOrCreateInstance<GameScene>( _serviceProvider );

        _timer = new Timer( state =>
        {
            _platform.UpdateCanvas();
        }, null, TimeSpan.FromMilliseconds( 1000 / 60.0 ), TimeSpan.FromMilliseconds( 1000 / 60.0 ) );
    }

    public void Paint( SKCanvas canvas, SKSizeI size, SKRectI dirtyRect )
    {
        if ( _currentScene.Frame.Size != size )
        {
            _currentScene.Frame = SKRectI.Create( _currentScene.Frame.Location, size );
        }

        canvas.Save();
        canvas.ClipRect( _currentScene.Frame );
        _currentScene.Draw( new DrawOperation( new PlatformCanvas( canvas ), _serviceProvider ) );
        canvas.Restore();
    }

    public void Pan( SKPointI amount )
    {
        if ( _currentScene is GameScene gameScene )
        {
            gameScene.Offset = new SKPointI( gameScene.Offset.X - amount.X, gameScene.Offset.Y - amount.Y );
            // _platform.UpdateCanvas();
        }
    }
}
