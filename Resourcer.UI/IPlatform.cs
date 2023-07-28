namespace Resourcer.UI;

/// <summary>
/// The interface that the game will use to perform specific platform
/// operations on the platform.
/// </summary>
public interface IPlatform
{
    /// <summary>
    /// Requests that the canvas be updated so that the game can draw new content.
    /// </summary>
    void UpdateCanvas();

    /// <summary>
    /// Invokes the action on the main thread.
    /// </summary>
    /// <param name="action">The action to be performed.</param>
    /// <returns>A task that indicates when the action has completed.</returns>
    Task InvokeOnMainThreadAsync( Func<Task> action );
}

