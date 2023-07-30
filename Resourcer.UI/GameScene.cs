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
    public override void Draw( SKCanvas canvas )
    {
        base.Draw( canvas );

        // Determine the X,Y map coordinates we will start painting from.
        var mapLeft = ( int ) Math.Floor( Offset.X / 64.0 );
        var mapTop = ( int ) Math.Floor( Offset.Y / 64.0 );
        var mapRight = ( int ) Math.Floor( ( Offset.X + Frame.Width ) / 64.0 );
        var mapBottom = ( int ) Math.Floor( ( Offset.Y + Frame.Height ) / 64.0 );

        if ( 10 < mapLeft || 10 > mapRight || 10 < mapTop || 10 > mapBottom )
        {
            return;
        }

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

        var x = left + ( ( 10 - mapLeft ) * 64 );
        var y = top + ( ( 10 - mapTop ) * 64 );

        var destination = new SKRect( x, y, x + 64, y + 64 );

        _sprites.CharacterTile.Draw( canvas, destination );
    }
}
