public class Biome
{
    public float HeightScale { get; private set; }
    public float FrequencyScale { get; private set; }
    public int OctavesCount { get; private set; }
    public float Exp { get; private set; }
    public float FirstOctaceValue { get; private set; }

    public Biome( float heightScale, float frequencyScale, float exp, float firstOctaceValue, int octavesCount)
    {
        FrequencyScale = frequencyScale;
        HeightScale = heightScale;
        Exp = exp;
        FirstOctaceValue = firstOctaceValue;
        OctavesCount = octavesCount;
    }
}
