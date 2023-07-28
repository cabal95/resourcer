using System.Windows;

using SkiaSharp.Views.Desktop;

namespace Resourcer.Platforms.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private IScene _currentScene;

    public MainWindow()
    {
        _currentScene = new GameScene();

        InitializeComponent();
    }

    protected void Canvas_PaintSurface( object sender, SKPaintSurfaceEventArgs e )
    {
        _currentScene.Draw( e.Surface.Canvas, new SkiaSharp.SKRect( 0, 0, e.Info.Width, e.Info.Height ) );
    }
}
