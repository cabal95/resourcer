﻿using SkiaSharp;

namespace OpenGameKit.Graphics;

/// <summary>
/// An object that can be drawn onto a canvas.
/// </summary>
public interface ISprite
{
    /// <summary>
    /// The native width of the sprite.
    /// </summary>
    int Width { get; }

    /// <summary>
    /// The native height of the sprite.
    /// </summary>
    int Height { get; }

    /// <summary>
    /// Draws the object onto the canvas inside the destination rectangle.
    /// </summary>
    /// <param name="canvas">The canvas surface this instance will be painted on.</param>
    /// <param name="destination">The rectangle on the surface that should be filled with this instance.</param>
    void Draw( SKCanvas canvas, SKRect destination );
}
