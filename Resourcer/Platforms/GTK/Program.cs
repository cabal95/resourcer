using Gtk;

namespace Resourcer.Platforms.Gtk;

public class Program
{
    private static IScene _currentScene = new GameScene();

    public static void Main( string[] args )
    {
        Application.Init();

        var myWin = new Window( "My GTK# App" );
        var scale = myWin.Screen.Resolution / 96.0;
        myWin.Resize( ( int ) ( 800 * scale ), ( int ) ( 450 * scale ) );

        myWin.Destroyed += MyWin_Destroyed;

        var box = new EventBox
        {
            CanFocus = true,
            FocusOnClick = true
        };
        box.AddEvents( ( int ) ( Gdk.EventMask.ButtonPressMask | Gdk.EventMask.KeyPressMask | Gdk.EventMask.PointerMotionMask ) );
        box.KeyPressEvent += Box_KeyPressEvent;
        box.ButtonPressEvent += Box_ButtonPressEvent;
        box.MotionNotifyEvent += Box_MotionNotifyEvent;

        myWin.Add( box );

        var canvas = new SkiaSharp.Views.Gtk.SKDrawingArea();
        canvas.PaintSurface += Canvas_PaintSurface;

        box.Add( canvas );

        myWin.ShowAll();

        Application.Run();
    }

    private static void Canvas_PaintSurface( object? sender, SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs e )
    {
        _currentScene.Draw( e.Surface.Canvas, new SkiaSharp.SKRect( 0, 0, e.Info.Width, e.Info.Height ) );
    }

    private static void Box_MotionNotifyEvent( object o, MotionNotifyEventArgs args )
    {
        System.Diagnostics.Debug.WriteLine( $"Move: {args.Event.X}x{args.Event.Y}" );
    }

    private static void Box_ButtonPressEvent( object o, ButtonPressEventArgs args )
    {
        System.Diagnostics.Debug.WriteLine( $"Press: {args.Event.X}x{args.Event.Y} {args.Event.Button}" );
    }

    private static void Box_KeyPressEvent( object o, KeyPressEventArgs args )
    {
        System.Diagnostics.Debug.WriteLine( $"Key: {args.Event.Key}" );
    }

    private static void MyWin_Destroyed( object? sender, EventArgs e )
    {
        Application.Quit();
    }
}
