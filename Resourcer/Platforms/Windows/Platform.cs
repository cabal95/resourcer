using Resourcer.UI;

using SkiaSharp.Views.WPF;

namespace Resourcer.Platforms.Windows;

/// <summary>
/// The logic for the Windows platform.
/// </summary>
public class Platform : IPlatform
{
    /// <summary>
    /// The canvas element that will be used when requesting paint updates.
    /// </summary>
    private readonly SKElement _canvas;

    /// <summary>
    /// Creates a new instance of <see cref="Platform"/>.
    /// </summary>
    /// <param name="canvas">The canvas element that will be used when requesting paint updates.</param>
    public Platform( SKElement canvas )
    {
        _canvas = canvas;
    }

    /// <inheritdoc/>
    public async Task InvokeOnMainThreadAsync( Func<Task> action )
    {
        var tcs = new TaskCompletionSource();

        _ = _canvas.Dispatcher.Invoke( async () =>
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

        await tcs.Task;
    }

    /// <inheritdoc/>
    public void UpdateCanvas()
    {
        try
        {
            _canvas.Dispatcher.Invoke( () => _canvas.InvalidateVisual(), System.Windows.Threading.DispatcherPriority.Render );
        }
        catch ( TaskCanceledException )
        {
            // Intentionally ignored, this happens during normal application shutdown.
        }
    }
}
