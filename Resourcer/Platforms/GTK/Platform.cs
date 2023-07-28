using Gtk;

using Resourcer.UI;

namespace Resourcer.Platforms.Gtk;

/// <summary>
/// The GTK platform logic.
/// </summary>
public class Platform : IPlatform
{
    /// <summary>
    /// The widget that will be used when requesting paint updates.
    /// </summary>
    private readonly Widget _canvas;

    /// <summary>
    /// Creates a new instance of <see cref="Platform"/>.
    /// </summary>
    /// <param name="canvas">The canvas widget that will be used when requesting paint updates.</param>
    public Platform( Widget canvas )
    {
        _canvas = canvas;
    }

    /// <inheritdoc/>
    public Task InvokeOnMainThreadAsync( Func<Task> action )
    {
        var tcs = new TaskCompletionSource();

        Application.Invoke( async (s, e) =>
        {
            try
            {
                await action();
                tcs.TrySetResult();
            }
            catch ( Exception ex )
            {
                tcs.TrySetException( ex );
            }
        } );

        return tcs.Task;
    }

    /// <inheritdoc/>
    public void UpdateCanvas()
    {
        Application.Invoke( ( s, e ) =>
        {
            _canvas.QueueDraw();
        } );
    }
}