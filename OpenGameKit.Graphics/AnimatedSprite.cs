﻿using OpenGameKit.Abstractions;

using SkiaSharp;

namespace OpenGameKit.Graphics;

/// <summary>
/// A sprite that is composed of other sprites and animates the change between
/// sprites over time.
/// </summary>
public class AnimatedSprite : ISprite
{
    /// <summary>
    /// The frame counter used to control our animation.
    /// </summary>
    private readonly IFrameCounter _frameCounter;

    /// <summary>
    /// The sprites that make up the animation sequence.
    /// </summary>
    private readonly ISprite[] _sprites;

    /// <inheritdoc/>
    public int Width => _sprites[0].Width;

    /// <inheritdoc/>
    public int Height => _sprites[0].Height;

    /// <summary>
    /// Creates a new instance of <see cref="AnimatedSprite"/>.
    /// </summary>
    /// <param name="sprites">The sprites that comprise the animation sequence.</param>
    public AnimatedSprite( IFrameCounter frameCounter, params ISprite[] sprites )
    {
        if ( sprites.Length == 0 )
        {
            throw new ArgumentException( "Must specify at least one sprite.", nameof( sprites ) );
        }

        _sprites = sprites;
        _frameCounter = frameCounter;
    }

    /// <inheritdoc/>
    public void Draw( SKCanvas canvas, SKRect destination )
    {
        var index = ( _frameCounter.Frame / 15 ) % _sprites.Length;

        _sprites[index].Draw( canvas, destination );
    }
}
