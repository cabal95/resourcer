using OpenGameKit.Abstractions;

namespace OpenGameKit;

/// <summary>
/// Extension methods for <see cref="IAnimationTimer"/>.
/// </summary>
public static class AnimationTimerExtensions
{
    /// <summary>
    /// Gets the frame index to display for the animation.
    /// </summary>
    /// <param name="animationTimer">The timer tracking game animation progress.</param>
    /// <param name="totalDuration">The total duration in seconds of one entire animation cycle of all frames.</param>
    /// <param name="frameCount">The number of frames in the animation.</param>
    /// <returns>A zero based index of the current frame number to display.</returns>
    public static int GetLinearFrame( this IAnimationTimer animationTimer, double totalDuration, int frameCount )
    {

        var frameDuration = totalDuration / frameCount;
        var value = animationTimer.Elapsed.TotalSeconds % totalDuration;

        var frame = ( int ) Math.Floor( value / frameDuration );

        return frame < frameCount ? frame : 0;
    }
}