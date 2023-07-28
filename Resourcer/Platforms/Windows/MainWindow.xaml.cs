﻿using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

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
    private SKPointI? _lastDragPosition;

    public MainWindow()
    {
        InitializeComponent();

        CanvasView.MouseDown += CanvasView_MouseDown;
        CanvasView.MouseUp += CanvasView_MouseUp;
        CanvasView.MouseMove += CanvasView_MouseMove;

        var serviceCollection = new ServiceCollection();

        serviceCollection.UseResourcerUI();
        serviceCollection.AddSingleton<IPlatform>( _ => new Platform( CanvasView ) );
        _engineServices = serviceCollection.BuildServiceProvider();

        _sceneManager = ActivatorUtilities.GetServiceOrCreateInstance<SceneManager>( _engineServices );

    }

    private SKPointI GetScaledPosition( MouseEventArgs e )
    {
        var dpi = VisualTreeHelper.GetDpi( CanvasView );
        var position = e.GetPosition( CanvasView );

        return new SKPointI( ( int ) Math.Round( position.X * dpi.DpiScaleX ), ( int ) Math.Round( position.Y * dpi.DpiScaleY ) );
    }

    private void CanvasView_MouseMove( object sender, System.Windows.Input.MouseEventArgs e )
    {
        if ( _lastDragPosition != null )
        {
            var position = GetScaledPosition( e );
            var offsetX = position.X - _lastDragPosition.Value.X;
            var offsetY = position.Y - _lastDragPosition.Value.Y;

            _lastDragPosition = position;
            _sceneManager.Pan( new SKPointI( offsetX, offsetY ) );
        }
    }

    private void CanvasView_MouseUp( object sender, MouseButtonEventArgs e )
    {
        if ( e.ChangedButton == MouseButton.Left )
        {
            _lastDragPosition = null;
        }
    }

    private void CanvasView_MouseDown( object sender, MouseButtonEventArgs e )
    {
        if ( e.ChangedButton == MouseButton.Left )
        {
            _lastDragPosition = GetScaledPosition( e );
        }
    }

    protected void Canvas_PaintSurface( object sender, SKPaintSurfaceEventArgs e )
    {
        _sceneManager.Paint( e.Surface.Canvas, new SKSizeI(e.Info.Width, e.Info.Height), new SKRectI( 0, 0, e.Info.Width, e.Info.Height ) );
    }
}
