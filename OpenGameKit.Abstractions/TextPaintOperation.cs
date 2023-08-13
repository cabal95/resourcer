namespace OpenGameKit.Abstractions;

/// <summary>
/// Describes how to paint text on the canvas.
/// </summary>
public class TextPaintOperation : PaintOperation
{
    /// <summary>
    /// The size of the font to be drawn.
    /// </summary>
    public float FontSize { get; set; } = 16;
}
