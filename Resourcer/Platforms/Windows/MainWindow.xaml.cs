using System.Windows;

using Microsoft.Extensions.DependencyInjection;

using Resourcer.UI;

using SkiaSharp;
using SkiaSharp.Views.Desktop;

namespace Resourcer.Platforms.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly IServiceProvider _engineServices;
    private readonly SceneManager _sceneManager;

    public MainWindow()
    {
        InitializeComponent();

        var serviceCollection = new ServiceCollection();

        serviceCollection.UseResourcerUI();
        serviceCollection.AddSingleton<IPlatform>( _ => new Platform( CanvasView ) );
        _engineServices = serviceCollection.BuildServiceProvider();

        _sceneManager = ActivatorUtilities.GetServiceOrCreateInstance<SceneManager>( _engineServices );

    }

    protected void Canvas_PaintSurface( object sender, SKPaintSurfaceEventArgs e )
    {
        _sceneManager.Paint( e.Surface.Canvas, new SKSizeI(e.Info.Width, e.Info.Height), new SKRectI( 0, 0, e.Info.Width, e.Info.Height ) );
    }
}
