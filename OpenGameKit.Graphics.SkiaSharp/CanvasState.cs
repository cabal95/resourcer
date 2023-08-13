namespace OpenGameKit.Graphics;

/// <summary>
/// Holds canvas state information that is not tracked internally by SkiaSharp.
/// </summary>
internal class CanvasState : IDisposable
{
    /// <summary>
    /// The action to call when this object is disposed.
    /// </summary>
    private readonly Action _disposeAction;

    /// <summary>
    /// The alpha value to be applied to all drawing operations.
    /// </summary>
    public float Alpha { get; set; } = 1.0f;

    /// <summary>
    /// Creates a new instance of <see cref="CanvasState"/>.
    /// </summary>
    /// <param name="originalState">The original state to populate this state from.</param>
    /// <param name="disposeAction">The action to call when disposed.</param>
    public CanvasState( CanvasState? originalState, Action disposeAction )
    {
        _disposeAction = disposeAction;

        if ( originalState != null )
        {
            CopyState( originalState, this );
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _disposeAction.Invoke();
    }

    /// <summary>
    /// Copies the data from one state into another state.
    /// </summary>
    /// <param name="fromState">The state to copy the values from.</param>
    /// <param name="toState">The state to copy the values into.</param>
    private static void CopyState( CanvasState fromState, CanvasState toState )
    {
        toState.Alpha = fromState.Alpha;
    }
}
