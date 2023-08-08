using System.Diagnostics;

using OpenGameKit.Abstractions;

namespace OpenGameKit;

/// <summary>
/// The default animation timer. It does not stop counting even when the
/// game engine is paused.
/// </summary>
public class AnimationTimer : IAnimationTimer
{
    /// <summary>
    /// The stop watch that tracks how long we have been running.
    /// </summary>
    private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

    /// <inheritdoc/>
    public TimeSpan Elapsed { get; private set; }

    /// <inheritdoc/>
    public void StartFrame()
    {
        Elapsed = _stopwatch.Elapsed;
    }

    /// <inheritdoc/>
    public void CompleteFrame()
    {
    }
}
