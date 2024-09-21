using System.Collections.Generic;

public class Biomes
{
    private static Biomes instance;

    private Biomes()
    {
        biomes = new Dictionary<BiomeType, Biome>();

        biomes[BiomeType.Forest]    = new Biome(10f, 2f, 0.1f, 0.9f,    2, new Biome.ChunkGrassData(0, 0.02f));
        biomes[BiomeType.LowHills]  = new Biome(11f, 0.7f, 0.5f, 1.5f,  3, new Biome.ChunkGrassData(0, 0.1f));
        biomes[BiomeType.Hills]     = new Biome(13, 0.6f, 0.5f, 2f,     2, new Biome.ChunkGrassData(0, 0.2f));
        biomes[BiomeType.HighHills] = new Biome(15f, 1.2f, 0.5f, 2.2f,  4, new Biome.ChunkGrassData(0, 0.3f));
        biomes[BiomeType.Mountains] = new Biome(18f, 0.7f, 0.5f, 2.8f,  5, new Biome.ChunkGrassData(0, 0.6f));
 
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
