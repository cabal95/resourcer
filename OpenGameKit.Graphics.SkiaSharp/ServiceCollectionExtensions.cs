using Microsoft.Extensions.DependencyInjection;

using OpenGameKit.Abstractions;
using OpenGameKit.Graphics.SkiaSharp;

namespace OpenGameKit.Graphics;

/// <summary>
/// Extension methods for <see cref="ServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers all the required services to use SkiaSharp as the game
    /// rendering engine.
    /// </summary>
    /// <param name="serviceCollection">The collection of services to register dependencies into.</param>
    public static void UseSkiaSharpRendering( this ServiceCollection serviceCollection )
    {
        serviceCollection.AddSingleton<ITextureProvider, PlatformTextureProvider>();
    }
}
