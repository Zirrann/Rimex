using System.Collections.Generic;

public class Biomes
{
    private static Biomes instance;

    private Biomes()
    {
        biomes = new Dictionary<BiomeType, Biome>();

        biomes[BiomeType.Forest] = new Biome(10f, 3f, 2f, 0.8f, 4);
        biomes[BiomeType.Hills] = new Biome(15f, 2f, 3f, 0.9f, 2);
        biomes[BiomeType.Mountains] = new Biome(100f, 4f, 3f, 1f, 5);
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
            return BiomeType.Forest;
        }
        else if (noiceValue < 0.6f)
        {
            return BiomeType.Hills;
        }
        else
        {
            return BiomeType.Mountains;
        }
    } 
}
