using System.Diagnostics;

using OpenGameKit.Abstractions;

namespace OpenGameKit;

/// <summary>
/// The default frame counter. It does not stop counting even when the
/// game engine is paused.
/// </summary>
public class FrameCounter : IFrameCounter
{
    /// <summary>
    /// The stop watch that tracks how long we have been running.
    /// </summary>
    private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

    /// <inheritdoc/>
    public int Frame => ( int ) Math.Floor( _stopwatch.Elapsed.TotalMilliseconds / ( 1_000 / 60.0 ) );
}
