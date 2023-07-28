using Gtk;

using Microsoft.Extensions.DependencyInjection;

using Resourcer.UI;

using SkiaSharp;
using SkiaSharp.Views.Desktop;

namespace Resourcer.Platforms.Gtk;

public class Program
{
    private static SceneManager? _sceneManager;

    public static void Main( string[] args )
    {
        Application.Init();

        var myWin = new Window( "Resourcer" );
        var scale = myWin.Screen.Resolution / 96.0;
        myWin.Resize( ( int ) ( 800 * scale ), ( int ) ( 450 * scale ) );
        myWin.SetPosition( WindowPosition.Center );

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

        var serviceCollection = new ServiceCollection();

        serviceCollection.UseResourcerUI();
        serviceCollection.AddSingleton<IPlatform>( _ => new Platform( canvas ) );

        var engineServices = serviceCollection.BuildServiceProvider();

        _sceneManager = ActivatorUtilities.GetServiceOrCreateInstance<SceneManager>( engineServices );


        myWin.ShowAll();

        try
        {
            Application.Run();
        }
        catch ( Exception ex )
        {
            System.Diagnostics.Debug.WriteLine( ex.Message );
            throw;
        }
    }

    private static void Canvas_PaintSurface( object? sender, SKPaintSurfaceEventArgs e )
    {
        _sceneManager?.Paint( e.Surface.Canvas, new SKSizeI( e.Info.Width, e.Info.Height ), new SKRectI( 0, 0, e.Info.Width, e.Info.Height ) );
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
