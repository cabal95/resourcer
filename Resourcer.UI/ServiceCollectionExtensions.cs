using Microsoft.Extensions.DependencyInjection;

using OpenGameKit;
using OpenGameKit.Abstractions;

namespace Resourcer.UI;

/// <summary>
/// Extension methods for <see cref="ServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers all the dependencies for the UI engine.
    /// </summary>
    /// <param name="serviceCollection">The collection of services to register dependencies into.</param>
    public static void UseResourcerUI( this ServiceCollection serviceCollection )
    {
        serviceCollection.AddSingleton<SpriteProvider>();
        serviceCollection.AddSingleton<IFrameCounter, FrameCounter>();
    }
}
