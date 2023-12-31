﻿using System.Drawing;

using Microsoft.Extensions.DependencyInjection;

using OpenGameKit.Abstractions;

namespace OpenGameKit.Graphics;

/// <summary>
/// A texture that is composed of other textures and animates the change between
/// textures over time.
/// </summary>
public class AnimatedTexture : ITexture
{
    /// <summary>
    /// The timer used to control our animation.
    /// </summary>
    private IAnimationTimer? _animationTimer;

    /// <summary>
    /// The textures that make up the animation sequence.
    /// </summary>
    private readonly ITexture[] _textures;

    /// <inheritdoc/>
    public int Width => _textures[0].Width;

    /// <inheritdoc/>
    public int Height => _textures[0].Height;

    /// <summary>
    /// Creates a new instance of <see cref="AnimatedTexture"/>.
    /// </summary>
    /// <param name="textures">The textures that comprise the animation sequence.</param>
    public AnimatedTexture( params ITexture[] textures )
    {
        if ( textures.Length == 0 )
        {
            throw new ArgumentException( "Must specify at least one texture.", nameof( textures ) );
        }

        _textures = textures;
    }

    /// <inheritdoc/>
    public void Draw( IDrawOperation operation, Rectangle destination )
    {
        _animationTimer ??= operation.GetRequiredService<IAnimationTimer>();

        var index = _animationTimer.GetLinearFrame( 0.75, _textures.Length );

        _textures[index].Draw( operation, destination );
    }
}
