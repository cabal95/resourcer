using System.Drawing;

using OpenGameKit.Abstractions;
using OpenGameKit.Graphics.SkiaSharp;

using SkiaSharp;

namespace OpenGameKit.Graphics;

/// <summary>
/// A canvas that supports drawing operations to SkiaSharp.
/// </summary>
public class SkiaSharpCanvas : ICanvas
{
    /// <summary>
    /// The native canvas object.
    /// </summary>
    private readonly SKCanvas _canvas;

    /// <summary>
    /// The stack of state objects.
    /// </summary>
    private readonly Stack<CanvasState> _states = new( 10 );

    /// <summary>
    /// The current state object.
    /// </summary>
    private CanvasState CurrentState => _states.Peek();

    /// <summary>
    /// Creates a new instance of <see cref="SkiaSharpCanvas"/>.
    /// </summary>
    /// <param name="canvas">The native SkiaSharp canvas.</param>
    public SkiaSharpCanvas( SKCanvas canvas )
    {
        _canvas = canvas;
        _states.Push( new CanvasState( null, PopState ) );
    }

    /// <inheritdoc/>
    public IDisposable SaveState()
    {
        var state = new CanvasState( CurrentState, PopState );

        _canvas.Save();
        _states.Push( state );

        return state;
    }

    /// <inheritdoc/>
    private void PopState()
    {
        if ( _states.Count <= 1 )
        {
            throw new InvalidOperationException( "Attempted to pop state when no state was saved." );
        }

        _canvas.Restore();
        _states.Pop();
    }

    /// <inheritdoc/>
    public void SetAlpha( float alpha )
    {
        CurrentState.Alpha = alpha;
    }

    /// <inheritdoc/>
    public void Scale( float scale )
    {
        _canvas.Scale( scale );
    }

    /// <inheritdoc/>
    public void Translate( float x, float y )
    {
        _canvas.Translate( x, y );
    }

    /// <inheritdoc/>
    public void ClipRect( RectangleF bounds )
    {
        _canvas.ClipRect( new SKRect( bounds.Left, bounds.Top, bounds.Right, bounds.Bottom ) );
    }

    /// <inheritdoc/>
    public void DrawRect( Rectangle rect, PaintOperation paint )
    {
        using var skiaPaint = new SKPaint();

        if ( paint.Fill.HasValue )
        {
            skiaPaint.ColorF = new SKColorF( paint.Fill.Value.R / 255.0f, paint.Fill.Value.G / 255.0f, paint.Fill.Value.B / 255.0f, paint.Fill.Value.A / 255.0f * CurrentState.Alpha );
        };

        _canvas.DrawRect( rect.X, rect.Y, rect.Width, rect.Height, skiaPaint );
    }

    /// <inheritdoc/>
    public void DrawTexture( ITexture texture, Rectangle destination )
    {
        if ( texture is not PlatformTexture platformTexture )
        {
            throw new ArgumentException( $"{nameof( DrawTexture )} must be called with a ${nameof( PlatformTexture )}.", nameof( texture ) );
        }

        var skiaDestination = new SKRect( destination.Left, destination.Top, destination.Right, destination.Bottom );

        using var paint = new SKPaint
        {
            ColorF = new SKColorF( 1, 1, 1, CurrentState.Alpha )
        };

        _canvas.DrawBitmap( platformTexture.Bitmap, platformTexture.SourceRect, skiaDestination, paint );
    }

    /// <inheritdoc/>
    public void DrawText( string text, Point location, TextPaintOperation paint )
    {
        using var skiaPaint = new SKPaint
        {
            TextSize = paint.FontSize
        };

        if ( paint.Fill.HasValue )
        {
            skiaPaint.ColorF = new SKColorF( paint.Fill.Value.R / 255.0f, paint.Fill.Value.G / 255.0f, paint.Fill.Value.B / 255.0f, paint.Fill.Value.A / 255.0f * CurrentState.Alpha );
        };

        _canvas.DrawText( text, location.X, location.Y + paint.FontSize, skiaPaint );
    }
}
