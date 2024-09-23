public abstract class Biome
{
    public float HeightScale { get; set; }
    public float FrequencyScale { get; set; }
    public int OctavesCount { get; set; }
    public float Exp { get; set; }
    public float FirstOctaveValue { get; set; } 

    public ChunkGrassData _GrassData;

    public struct ChunkGrassData
    {
        public float GrassIslandHeight { get; set; } 
        public float GrassPointSkipValue { get; set; }

        public ChunkGrassData(float grassIslandHeight, float grassPointSkipValue)
        {
            GrassIslandHeight = grassIslandHeight;
            GrassPointSkipValue = grassPointSkipValue;
        }
    }

    public Biome(float heightScale, float frequencyScale, float firstOctaceValue, float exp, int octavesCount, ChunkGrassData grassData)
    {
        HeightScale = heightScale;
        FrequencyScale = frequencyScale;
        OctavesCount = octavesCount;
        Exp = exp;
        FirstOctaveValue = firstOctaceValue;
        _GrassData = grassData;
    }

    public Biome( float heightScale, float frequencyScale, float firstOctaceValue, float exp, int octavesCount)
    {
        FrequencyScale = frequencyScale;
        HeightScale = heightScale;
        Exp = exp;
        FirstOctaveValue = firstOctaceValue;
        OctavesCount = octavesCount;
    }

    public abstract void GenerateBiomeObjectsForChunk(Chunk chunk);
    
}
