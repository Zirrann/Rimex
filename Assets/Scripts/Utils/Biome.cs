public class Biome
{
    public float HeightScale { get; private set; }
    public float FrequencyScale { get; private set; }
    public int OctavesCount { get; private set; }
    public float Exp { get; private set; }
    public float FirstOctaceValue { get; private set; } 

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
        FirstOctaceValue = firstOctaceValue;
        _GrassData = grassData;
    }

    public Biome( float heightScale, float frequencyScale, float firstOctaceValue, float exp, int octavesCount)
    {
        FrequencyScale = frequencyScale;
        HeightScale = heightScale;
        Exp = exp;
        FirstOctaceValue = firstOctaceValue;
        OctavesCount = octavesCount;
    }
}
