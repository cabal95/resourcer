using OpenGameKit.Graphics;

using Resourcer.UI;

using SkiaSharp;

namespace Resourcer;

public class GameScene : Scene
{
    private readonly MapScene _mapScene;

    public SKPointI Offset
    {
        get => _mapScene.Offset;
        set => _mapScene.Offset = value;
    }

    private readonly SpriteProvider _sprites;

    private readonly System.Diagnostics.Stopwatch _fpsStopwatch = System.Diagnostics.Stopwatch.StartNew();

    private int _frames = 0;

    private int _fps = 0;

    public GameScene( SpriteProvider sprites )
    {
        _mapScene = new MapScene( sprites );
        _sprites = sprites;

        Children.Add( _mapScene );
    }

    /// <inheritdoc/>
    protected override void Layout()
    {
        _mapScene.Frame = SKRectI.Create( 0, 0, Frame.Width, Frame.Height );
    }

    /// <inheritdoc/>
    public override void Draw( IDrawOperation operation )
    {
        base.Draw( operation );

        var posX = 12;
        var posY = 12;

        // Determine the X,Y map coordinates we will start painting from.
        var mapLeft = ( int ) Math.Floor( Offset.X / 64.0 );
        var mapTop = ( int ) Math.Floor( Offset.Y / 64.0 );
        var mapRight = ( int ) Math.Floor( ( Offset.X + Frame.Width ) / 64.0 );
        var mapBottom = ( int ) Math.Floor( ( Offset.Y + Frame.Height ) / 64.0 );

        if ( posX >= mapLeft && posX <= mapRight && posY >= mapTop && posY <= mapBottom )
        {
            // Determine the tile offset.
            var tileOffsetX = Offset.X % 64 != 0
                ? Offset.X < 0 ? 64 + ( Offset.X % 64 ) : Offset.X % 64
                : 0;
            var tileOffsetY = Offset.Y % 64 != 0
                ? Offset.Y < 0 ? 64 + ( Offset.Y % 64 ) : Offset.Y % 64
                : 0;

            // Determine the starting painting position.
            int left = Frame.Left - tileOffsetX;
            int top = Frame.Top - tileOffsetY;

            var x = left + ( ( posX - mapLeft ) * 64 );
            var y = top + ( ( posY - mapTop ) * 64 );

            var destination = new SKRect( x + 8, y + 8, x + 48, y + 48 );

            _sprites.CharacterTile.Draw( operation, destination );
        }

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
