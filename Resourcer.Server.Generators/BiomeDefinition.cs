namespace Resourcer.Server.Generators;

/// <summary>
/// Defines the requirements that must be met for a Biome when generating
/// the map cells.
/// </summary>
public class BiomeDefinition
{
    /// <summary>
    /// The identifier of the biome.
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// The minimum height the cell must have to be a match.
    /// </summary>
    public float MinHeight { get; }

    /// <summary>
    /// The maximum height the cell must have to be a match.
    /// </summary>
    public float MaxHeight { get; }

    /// <summary>
    /// The minimum moisture the cell must have to be a match.
    /// </summary>
    public float MinMoisture { get; }

    /// <summary>
    /// The maximum moisture the cell must have to be a match.
    /// </summary>
    public float MaxMoisture { get; }

    /// <summary>
    /// The minimum heat the cell must have to be a match.
    /// </summary>
    public float MinHeat { get; }

    /// <summary>
    /// The maximum heat the cell must have to be a match.
    /// </summary>
    public float MaxHeat { get; }

    /// <summary>
    /// Creates a new biome definition that is used to match a map cell with
    /// the proper biome data.
    /// </summary>
    /// <param name="id">The identifier of the biome.</param>
    /// <param name="minHeight">The minimum height the cell must have to match.</param>
    /// <param name="minMoisture">The minimum moisture the cell must have to match.</param>
    /// <param name="minHeat">The minimum heat the cell must have to match.</param>
    public BiomeDefinition( int id, float minHeight, float minMoisture, float minHeat, float maxHeight = 1.0f, float maxMoisture = 1.0f, float maxHeat = 1.0f )
    {
        Id = id;
        MinHeight = minHeight;
        MinMoisture = minMoisture;
        MinHeat = minHeat;
        MaxHeight = maxHeight;
        MaxMoisture = maxMoisture;
        MaxHeat = maxHeat;
    }
    
    /// <summary>
    /// Determines if the cell values match this preset.
    /// </summary>
    /// <param name="height">The cell height value.</param>
    /// <param name="moisture">The cell moisture value.</param>
    /// <param name="heat">The cell heat value.</param>
    /// <returns><c>true</c> if the cell values match the biome requirements.</returns>
    public bool Matches( float height, float moisture, float heat )
    {
        return height >= MinHeight && height <= MaxHeight
            && moisture >= MinMoisture && moisture <= MaxMoisture
            && heat >= MinHeat && heat <= MaxHeat;
    }

    /// <summary>
    /// Gets the difference from the ideal range the cell is from this biome.
    /// </summary>
    /// <param name="height">The cell height value.</param>
    /// <param name="moisture">The cell moisture value.</param>
    /// <param name="heat">The cell heat value.</param>
    /// <returns>The distance from the ideal value.</returns>
    public float GetDiffValue( float height, float moisture, float heat )
    {
        var midHeight = ( ( MaxHeight - MinHeight ) / 2.0f ) + MinHeight;
        var midMoisture = ( ( MaxMoisture - MinMoisture ) / 2.0f ) + MinMoisture;
        var midHeat = ( ( MaxHeat - MinHeat ) / 2.0f ) + MinHeat;

        return Math.Abs( ( height - midHeight ) + ( moisture - midMoisture ) + ( heat - midHeat ) );
    }
}
