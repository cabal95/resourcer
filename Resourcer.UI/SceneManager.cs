using System.Drawing;

using Microsoft.Extensions.DependencyInjection;

using OpenGameKit.Abstractions;
using OpenGameKit.Graphics;

using SkiaSharp;

namespace Resourcer.UI;

public class SceneManager
{
    private readonly IPlatform _platform;

    private readonly IScene _currentScene;

    private readonly IServiceProvider _serviceProvider;

    private readonly IAnimationTimer _animationTimer;

    private readonly Timer _timer;

    public SceneManager( IServiceProvider serviceProvider, IPlatform platform, IAnimationTimer animationTimer )
    {
        _serviceProvider = serviceProvider;
        _platform = platform;
        _animationTimer = animationTimer;

        _currentScene = ActivatorUtilities.GetServiceOrCreateInstance<GameScene>( _serviceProvider );

        _timer = new Timer( state =>
        {
            _platform.UpdateCanvas();
        }, null, TimeSpan.FromMilliseconds( 1000 / 60.0 ), TimeSpan.FromMilliseconds( 1000 / 60.0 ) );
    }

    public void Paint( SKCanvas canvas, SKSizeI size, SKRectI dirtyRect )
    {
        if ( _currentScene.Frame.Width != size.Width || _currentScene.Frame.Height != size.Height )
        {
            _currentScene.Frame = new Rectangle( _currentScene.Frame.Location, new Size( size.Width, size.Height ) );
        }

        _animationTimer.StartFrame();

        canvas.Save();
        canvas.ClipRect( new SKRect( _currentScene.Frame.Left, _currentScene.Frame.Top, _currentScene.Frame.Right, _currentScene.Frame.Bottom ) );
        canvas.Clear();
        _currentScene.Draw( new DrawOperation( new SkiaSharpCanvas( canvas ), _serviceProvider ) );
        canvas.Restore();
    }

    public void Pan( SKPointI amount )
    {
        if ( _currentScene is GameScene gameScene )
        {
            gameScene.Offset = new SKPointI( gameScene.Offset.X - amount.X, gameScene.Offset.Y - amount.Y );
        }
    }
}
