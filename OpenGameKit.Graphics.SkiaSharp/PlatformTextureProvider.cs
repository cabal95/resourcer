using System.Drawing;

using OpenGameKit.Abstractions;

using SkiaSharp;

namespace OpenGameKit.Graphics.SkiaSharp;

/// <summary>
/// SkiaSharp platform rendering texture provider.
/// </summary>
internal class PlatformTextureProvider : ITextureProvider
{
    /// <inheritdoc/>
    public ITexture LoadTexture( Stream stream )
    {
        var source = SKBitmap.Decode( stream );
        source.SetImmutable();

        return new PlatformTexture( source, new Rectangle( 0, 0, source.Width, source.Height ) );
    }

    /// <inheritdoc/>
    public ITextureSheet LoadTextureSheet( Stream stream )
    {
        return new PlatformTextureSheet( stream );
    }
}
