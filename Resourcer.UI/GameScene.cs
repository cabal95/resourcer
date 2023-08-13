using System.Drawing;

using OpenGameKit.Abstractions;
using OpenGameKit.Graphics;

using Resourcer.UI;

using SkiaSharp;

namespace Resourcer;

public class GameScene : Scene
{
    private Point _offset;

    private readonly MapScene _mapScene;
    private readonly CharacterScene _characterScene;

    public Point Offset
    {
        get => _offset;
        set
        {
            _offset = value;
            _mapScene.Offset = value;
            _characterScene.Offset = value;
        }
    }

    private readonly SpriteProvider _sprites;

    private readonly System.Diagnostics.Stopwatch _fpsStopwatch = System.Diagnostics.Stopwatch.StartNew();

    private int _frames = 0;

    private int _fps = 0;

    public GameScene( SpriteProvider sprites )
    {
        _mapScene = new MapScene( sprites );
        _characterScene = new CharacterScene( sprites );
        _sprites = sprites;

        Children.Add( _mapScene );
        Children.Add( _characterScene );
    }

    /// <inheritdoc/>
    protected override void Layout()
    {
        _characterScene.Frame = _mapScene.Frame = new Rectangle( 0, 0, Frame.Width, Frame.Height );
    }

    /// <inheritdoc/>
    protected override void DrawContent( IDrawOperation operation )
    {
        _frames++;

        if ( operation.GetService( typeof( SKCanvas ) ) is SKCanvas canvas )
        {
            using var paint = new SKPaint();

            paint.TextSize = 16.0f;
            paint.IsAntialias = true;
            paint.IsStroke = false;

            if ( _fpsStopwatch.ElapsedMilliseconds > 1000 )
            {
                _fps = ( int ) Math.Round( _frames / _fpsStopwatch.Elapsed.TotalSeconds );
            }

            var text = $"FPS: {_fps}";
            var textWidth = paint.MeasureText( text );

            paint.Color = new SKColor( 0, 0, 0, 64 );
            canvas.DrawRect( SKRect.Create( 10, 10, textWidth, 18 ), paint );

            paint.Color = SKColors.White;
            canvas.DrawText( text, 10, 10 + 16, paint );
        }

        if ( _fpsStopwatch.ElapsedMilliseconds > 10_000 )
        {
            _fpsStopwatch.Restart();
            _frames = 0;
        }
    }
}
