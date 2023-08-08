namespace OpenGameKit.Abstractions;

/// <summary>
/// Provides a way for UI elements to access information about the current
/// drawing operation.
/// </summary>
public interface IDrawOperation : IServiceProvider
{
    public ICanvas Canvas { get; }
}
