namespace Resourcer.Server.Generators;

public class BiomePreset
{
    public int Id { get; set; }

    public float MinHeight { get; set; }

    public float MinMoisture { get; set; }

    public float MinHeat { get; set; }

    public bool Matches( float height, float moisture, float heat )
    {
        return height >= MinHeight && moisture >= MinMoisture && heat >= MinHeat;
    }

    public float GetDiffValue( float height, float moisture, float heat )
    {
        return ( height - MinHeight ) + ( moisture - MinMoisture ) + ( heat - MinHeat );
    }
}