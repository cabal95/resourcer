using System.Collections.ObjectModel;

using Microsoft.Extensions.DependencyInjection;

using SkiaSharp;

namespace OpenGameKit.Graphics;
/// <summary>
/// A scene that contains child scenes.
/// </summary>
public class Scene : IScene
{
    /// <summary>
    /// Will be <c>true</c> if layout has been requested.
    /// </summary>
    private bool _layoutRequested;

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
            RequestLayout();
        }
    }

    /// <summary>
    /// The set of child scenes that will be displayed inside this scene.
    /// The child scenes will be drawn in order, meaning the last item
    /// in the list will have the highest z-index.
    /// </summary>
    public IList<IElement> Children { get; }

    /// <summary>
    /// Creates a new instance of <see cref="Scene"/>.
    /// </summary>
    public Scene()
    {
        var elementCollection = new ObservableCollection<IElement>();

        elementCollection.CollectionChanged += ( s, e ) => RequestLayout();

        Children = elementCollection;
    }

    /// <inheritdoc/>
    public virtual void RequestLayout()
    {
        _layoutRequested = true;
    }

    /// <summary>
    /// Calls the <see cref="Layout()"/> method if layout has been requested.
    /// </summary>
    protected void UpdateLayoutIfRequested()
    {
        if ( _layoutRequested )
        {
            _layoutRequested = false;
            Layout();
        }
    }

    /// <summary>
    /// Performs a layout of all child scenes.
    /// </summary>
    protected virtual void Layout()
    {
        // Do nothing, the generic use scene expects children to be laid
        // out manually or by child classes.
    }

    /// <inheritdoc/>
    public SKSizeI GetDesiredSize( int widthConstraint, int heightConstraint )
    {
        return new SKSizeI( widthConstraint, heightConstraint );
    }

    /// <inheritdoc/>
    public virtual void Draw( IDrawOperation operation )
    {
        UpdateLayoutIfRequested();

        foreach ( var child in Children )
        {
            //canvas.Save();
            //canvas.ClipRect( child.Frame );

            child.Draw( operation );

            //canvas.Restore();
        }
    }
}
