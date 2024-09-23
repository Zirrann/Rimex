public class HillsBiome : Biome
{
    public HillsBiome() : base(0, 0, 0, 0, 0)
    {
        HeightScale = 13f;
        FrequencyScale = 0.6f;
        FirstOctaveValue = 0.5f;
        Exp = 2f;
        OctavesCount = 2;
        _GrassData = new ChunkGrassData(0, 0.2f);
    }

    public override void GenerateBiomeObjectsForChunk(Chunk chunk)
    {
        throw new System.NotImplementedException();
    }
}
