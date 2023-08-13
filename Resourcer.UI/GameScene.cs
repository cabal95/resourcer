using System.Drawing;

using OpenGameKit.Abstractions;
using OpenGameKit.Graphics;

using Resourcer.UI;

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

        if ( _fpsStopwatch.ElapsedMilliseconds > 1000 )
        {
            _fps = ( int ) Math.Round( _frames / _fpsStopwatch.Elapsed.TotalSeconds );
        }

        var text = $"FPS: {_fps}";
        var textWidth = 100;

        operation.Canvas.DrawRect( new Rectangle( 10, 10, 10 + textWidth, 20 ), new PaintOperation
        {
            Fill = Color.FromArgb( 64, 0, 0, 0 )
        } );

        operation.Canvas.DrawText( text, new Point( 14, 10 ), new TextPaintOperation
        {
            Fill = Color.White,
            FontSize = 16
        } );

        if ( _fpsStopwatch.ElapsedMilliseconds > 10_000 )
        {
            _fpsStopwatch.Restart();
            _frames = 0;
        }
    }
}
