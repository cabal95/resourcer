namespace OpenGameKit.Abstractions;

/// <summary>
/// Provides functionality to track the a common timer that all animations can
/// use to determine which animated frame to show.
/// </summary>
public interface IAnimationTimer
{
    /// <summary>
    /// The amount of time that has elapsed since the timer
    /// started.
    /// </summary>
    TimeSpan Elapsed { get; }

    /// <summary>
    /// Called when a new frame is about to be rendered. This provides a
    /// snapshot to the current elapsed value so that all calls until the
    /// next frame return the same value.
    /// </summary>
    void StartFrame();

    /// <summary>
    /// Called when the current frame has completed painting.
    /// </summary>
    void CompleteFrame();
}
