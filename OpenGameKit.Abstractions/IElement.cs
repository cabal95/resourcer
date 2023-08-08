using System.Drawing;

namespace OpenGameKit.Abstractions;

/// <summary>
/// An element describes and paints a single UI element.
/// </summary>
public interface IElement
{
    /// <summary>
    /// The current position and size of the element.
    /// </summary>
    Rectangle Frame { get; set; }

    /// <summary>
    /// Calculates and returns the desired size of the element given the width
    /// height constraints. It is not guaranteed that the return value will
    /// be used for the element frame.
    /// </summary>
    /// <param name="widthConstraint">The maximum number of pixels wide the element can be.</param>
    /// <param name="heightConstraint">The maximum number of pixels high the element can be.</param>
    /// <returns>The size that the element is requesting.</returns>
    Size GetDesiredSize( int widthConstraint, int heightConstraint );

    /// <summary>
    /// Draws the element.
    /// </summary>
    /// <param name="operation">The current drawing operation.</param>
    void Draw( IDrawOperation operation );
}
