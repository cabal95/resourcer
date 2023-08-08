namespace OpenGameKit.Abstractions;

/// <summary>
/// Provides functionality to track the number of animation frames that have
/// elapsed since the engine started.
/// </summary>
public interface IFrameCounter
{
    /// <summary>
    /// The number of frames that have elapsed.
    /// </summary>
    public int Frame { get; }
}
