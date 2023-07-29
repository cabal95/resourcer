using SkiaSharp;

namespace OpenGameKit.Graphics;

/// <summary>
/// A scene that contains child scenes.
/// </summary>
public class LayoutScene : IScene
{
    /// <summary>
    /// The current frame for this scene.
    /// </summary>
    private SKRectI _frame = SKRectI.Empty;

    /// <inheritdoc/>
    public SKRectI Frame
    {
        get => _frame;
        set
        {
            _frame = value;
            Layout();
        }
    }

    /// <summary>
    /// The set of child scenes that will be displayed inside this scene.
    /// The child scenes will be drawn in order, meaning the last item
    /// in the list will have the highest z-index.
    /// </summary>
    public IList<IScene> Children { get; } = new List<IScene>();

    /// <summary>
    /// Performs a layout of all child scenes.
    /// </summary>
    public virtual void Layout()
    {
        foreach ( var child in Children )
        {
            var location = child.Frame.Location;
            var size = child.GetDesiredSize( Frame.Width - location.X, Frame.Height - location.Y );

            if ( child.Frame.Size != size )
            {
                child.Frame = SKRectI.Create( location, size );
            }
        }
    }

    /// <inheritdoc/>
    public SKSizeI GetDesiredSize( int widthConstraint, int heightConstraint )
    {
        return new SKSizeI( widthConstraint, heightConstraint );
    }

    /// <inheritdoc/>
    public virtual void Draw( SKCanvas canvas )
    {
        foreach ( var child in Children )
        {
            canvas.Save();
            canvas.ClipRect( child.Frame );
            child.Draw( canvas );
            canvas.Restore();
        }
    }
}
