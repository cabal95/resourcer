using SkiaSharp;

namespace GameMap;

public partial class MainPage : ContentPage
{
	public GameScene Scene { get; }

	private PointF? _lastInteractionPostion;
	
	public MainPage()
	{
		Scene = new GameScene();

		InitializeComponent();

		Task.Run( async () =>
		{
			while ( true )
			{
				await Task.Delay( 1000 );

				await Dispatcher.DispatchAsync( () =>
				{
					System.Diagnostics.Debug.WriteLine( $"Size = {SceneView.Width}x{SceneView.Height}" );
					SceneView.InvalidateSurface();
				} );
			}
		} );
	}

    private void SceneView_StartInteraction( object sender, TouchEventArgs e )
    {
		_lastInteractionPostion = new PointF( e.Touches[0].X, e.Touches[0].Y );
    }

    private void SceneView_DragInteraction( object sender, TouchEventArgs e )
    {
		var changeX = e.Touches[0].X - _lastInteractionPostion.Value.X;
		var changeY = e.Touches[0].Y - _lastInteractionPostion.Value.Y;

		_lastInteractionPostion = new PointF( e.Touches[0].X, e.Touches[0].Y );

		Dispatcher.DispatchAsync( () =>
		{
			Scene.Offset = new PointF( Scene.Offset.X + changeX, Scene.Offset.Y + changeY );
			SceneView.InvalidateSurface();
		} );
    }

    private void SKCanvasView_PaintSurface( object sender, SkiaSharp.Views.Maui.SKPaintSurfaceEventArgs e )
    {
		e.Surface.Canvas.Save();
		e.Surface.Canvas.ClipRect( new SKRect( 0, 0, e.Info.Width, e.Info.Height ) );

        e.Surface.Canvas.Clear();
        Scene.Draw( e.Surface.Canvas, new RectF( 0, 0, e.Info.Width, e.Info.Height ) );

		e.Surface.Canvas.Restore();
    }

    private void SceneView_Touch( object sender, SkiaSharp.Views.Maui.SKTouchEventArgs e )
    {
		if ( e.ActionType == SkiaSharp.Views.Maui.SKTouchAction.Pressed )
		{
			_lastInteractionPostion = new PointF( e.Location.X, e.Location.Y );
		}
		else if ( e.ActionType == SkiaSharp.Views.Maui.SKTouchAction.Released )
		{
			_lastInteractionPostion = null;
		}
		else if ( e.ActionType == SkiaSharp.Views.Maui.SKTouchAction.Moved && _lastInteractionPostion.HasValue )
		{
			var changeX = e.Location.X - _lastInteractionPostion.Value.X;
			var changeY = e.Location.Y - _lastInteractionPostion.Value.Y;

			_lastInteractionPostion = new PointF( e.Location.X, e.Location.Y );

			Dispatcher.DispatchAsync( () =>
			{
				Scene.Offset = new PointF( Scene.Offset.X + changeX, Scene.Offset.Y + changeY );
				SceneView.InvalidateSurface();
			} );
		}
    }
}

