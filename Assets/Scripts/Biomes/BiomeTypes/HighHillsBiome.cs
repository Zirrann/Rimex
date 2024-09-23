public class HighHillsBiome : Biome
{
    public HighHillsBiome() : base(0, 0, 0, 0, 0)
    {
        HeightScale = 15f;
        FrequencyScale = 1.2f;
        FirstOctaveValue = 0.5f;
        Exp = 2.2f;
        OctavesCount = 4;
        _GrassData = new ChunkGrassData(0, 0.3f);
    }

    public override Chunk GenerateBiomeObjectsForChunk(Chunk chunk)
    {
        throw new System.NotImplementedException();
    }
}
