
public class ForestBiome : Biome
{
    public ForestBiome() : base(0, 0, 0, 0, 0)
    {
        HeightScale = 10f;
        FrequencyScale = 2f;
        FirstOctaveValue = 0.1f;
        Exp = 0.9f;
        OctavesCount = 3;
        _GrassData = new ChunkGrassData(0, 0.02f);
    }

    public override Chunk GenerateBiomeObjectsForChunk(Chunk chunk)
    {
        throw new System.NotImplementedException();
    }
}
