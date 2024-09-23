public class LowHillsBiome : Biome
{
    public LowHillsBiome() : base(0, 0, 0, 0, 0)
    {
        HeightScale = 11f;
        FrequencyScale = 0.7f;
        FirstOctaveValue = 0.5f;
        Exp = 1.5f;
        OctavesCount = 3;
        _GrassData = new ChunkGrassData(0, 0.1f);
    }

    public override Chunk GenerateBiomeObjectsForChunk(Chunk chunk)
    {
        throw new System.NotImplementedException();
    }

}
