using System.Collections.Generic;

public class Biomes
{
    private static Biomes instance;

    private Biomes()
    {
        biomes = new Dictionary<BiomeType, Biome>();

        biomes[BiomeType.Forest]    = new ForestBiome();
        biomes[BiomeType.LowHills]  = new LowHillsBiome();
        biomes[BiomeType.Hills]     = new HillsBiome();
        biomes[BiomeType.HighHills] = new HighHillsBiome();
        biomes[BiomeType.Mountains] = new MountainsBiome();
 
    }

    public static Biomes Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Biomes();
            }
            return instance;
        }
    }

    private Dictionary<BiomeType, Biome> biomes;

    public Biome GetBiome(BiomeType type)
    {
        return biomes[type];
    }


    public static BiomeType GetBiomeType(float noiceValue) {

        if (noiceValue < 0.3f)
        {
            return BiomeType.Mountains;
        }
       else if (noiceValue < 0.4f)
        {
            return BiomeType.HighHills;
        }
        else if (noiceValue < 0.55f)
        {
            return BiomeType.Hills;
        }
        else if (noiceValue < 0.7f)
        {
            return BiomeType.LowHills;
        }
        else
        {
            return BiomeType.Forest;
        }
    } 
}
