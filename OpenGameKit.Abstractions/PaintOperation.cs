using System.Drawing;

namespace OpenGameKit.Abstractions;

/// <summary>
/// Describes how to paint something on the canvas.
/// </summary>
public class PaintOperation
{
    /// <summary>
    /// The color to use when performing the fill.
    /// </summary>
    public Color? Fill { get; set; }
}
