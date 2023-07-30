namespace OpenGameKit.Graphics;

/// <summary>
/// A scene describes and handles layout for a group of UI elements.
/// </summary>
public interface IScene : IElement
{
    /// <summary>
    /// The child elements that make up this scene.
    /// </summary>
    IList<IElement> Children { get; }

    /// <summary>
    /// Requests that the layout of the scene be updated before the next render
    /// cycle is performed.
    /// </summary>
    void RequestLayout();
}
