using OpenGameKit.Abstractions;

namespace OpenGameKit;

/// <summary>
/// Extension methods for <see cref="IAnimationTimer"/>.
/// </summary>
public static class AnimationTimerExtensions
{
    public static int GetLinearFrame( this IAnimationTimer animationTimer, double totalDuration, int frameCount )
    {
        var frameDuration = totalDuration / frameCount;
        var value = animationTimer.Elapsed.TotalSeconds % totalDuration;

        var frame = ( int ) Math.Floor( value / frameDuration );

        return frame < frameCount ? frame : 0;
    }
}