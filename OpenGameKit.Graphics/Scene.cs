using System.Collections.ObjectModel;
using System.Drawing;

using OpenGameKit.Abstractions;

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
    private Rectangle _frame = Rectangle.Empty;

    /// <inheritdoc/>
    public Rectangle Frame
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
    public Size GetDesiredSize( int widthConstraint, int heightConstraint )
    {
        return new Size( widthConstraint, heightConstraint );
    }

    /// <inheritdoc/>
    public virtual void Draw( IDrawOperation operation )
    {
        UpdateLayoutIfRequested();

        DrawChildren( operation );
        DrawContent( operation );
    }

    /// <summary>
    /// Draws all the children elements of this scene.
    /// </summary>
    /// <param name="operation">The current drawing operation.</param>
    protected virtual void DrawChildren( IDrawOperation operation )
    {
        foreach ( var child in Children )
        {
            using var state = operation.Canvas.SaveState();

            operation.Canvas.ClipRect( child.Frame );
            operation.Canvas.Translate( child.Frame.X, child.Frame.Y );

            child.Draw( operation );
        }
    }

    /// <summary>
    /// Draws all the scene inner content. This is called after the children
    /// have been drawn.
    /// </summary>
    /// <param name="operation">The current drawing operation.</param>
    protected virtual void DrawContent( IDrawOperation operation )
    {
    }
}
