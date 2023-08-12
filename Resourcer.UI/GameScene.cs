using System.Drawing;

using OpenGameKit.Abstractions;
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
        _mapScene.Frame = new Rectangle( 0, 0, Frame.Width, Frame.Height );
    }

    /// <inheritdoc/>
    public override void Draw( IDrawOperation operation )
    {
        base.Draw( operation );

        // Determine the X,Y map coordinates we will start painting from.
        var mapLeft = ( int ) Math.Floor( Offset.X / 64.0 );
        var mapTop = ( int ) Math.Floor( Offset.Y / 64.0 );
        var mapRight = ( int ) Math.Floor( ( Offset.X + Frame.Width ) / 64.0 );
        var mapBottom = ( int ) Math.Floor( ( Offset.Y + Frame.Height ) / 64.0 );

        // Determine the drawing offsets.
        var drawingOffsetX = ( mapLeft * 64 ) - Offset.X;
        var drawingOffsetY = ( mapTop * 64 ) - Offset.Y;

        var characterPosX = 12;
        var characterPosY = 12;

        if ( characterPosX >= mapLeft && characterPosX <= mapRight && characterPosY >= mapTop && characterPosY <= mapBottom )
        {
            // Determine the starting painting position.
            int left = Frame.Left + drawingOffsetX;
            int top = Frame.Top + drawingOffsetY;

            var x = left + ( ( characterPosX - mapLeft ) * 64 );
            var y = top + ( ( characterPosY - mapTop ) * 64 );

            var destination = new Rectangle( x + 8, y + 8, 48, 48 );

            _sprites.CharacterTile.Draw( operation, destination );
        }

        var copperPosX = 13;
        var copperPosY = 12;

        if ( copperPosX >= mapLeft && copperPosX <= mapRight && copperPosY >= mapTop && copperPosY <= mapBottom )
        {
            // Determine the starting painting position.
            int left = Frame.Left + drawingOffsetX;
            int top = Frame.Top + drawingOffsetY;

            var x = left + ( ( copperPosX - mapLeft ) * 64 );
            var y = top + ( ( copperPosY - mapTop ) * 64 );

            var destination = new Rectangle( x, y, 64, 64 );

            _sprites.CopperResource.Draw( operation, destination );
        }

        var cobaltPosX = 13;
        var cobaltPosY = 13;

        if ( cobaltPosX >= mapLeft && cobaltPosX <= mapRight && cobaltPosY >= mapTop && cobaltPosY <= mapBottom )
        {
            // Determine the starting painting position.
            int left = Frame.Left + drawingOffsetX;
            int top = Frame.Top + drawingOffsetY;

            var x = left + ( ( cobaltPosX - mapLeft ) * 64 );
            var y = top + ( ( cobaltPosY - mapTop ) * 64 );

            var destination = new Rectangle( x, y, 64, 64 );

            _sprites.CobaltResource.Draw( operation, destination );
        }

        var miningPosX = 13;
        var miningPosY = 12;

        if ( miningPosX >= mapLeft && miningPosX <= mapRight && miningPosY >= mapTop && miningPosY <= mapBottom )
        {
            // Determine the starting painting position.
            int left = Frame.Left + drawingOffsetX;
            int top = Frame.Top + drawingOffsetY;

            var x = left + ( ( miningPosX - mapLeft ) * 64 );
            var y = top + ( ( miningPosY - mapTop ) * 64 );

            var destination = new Rectangle( x, y, 128, 128 );

            _sprites.MiningFacility.Draw( operation, destination );
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
