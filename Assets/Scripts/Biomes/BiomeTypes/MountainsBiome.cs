public class MountainsBiome : Biome
{
    public MountainsBiome() : base(0, 0, 0, 0, 0)
    {
        HeightScale = 18f;
        FrequencyScale = 0.7f;
        FirstOctaveValue = 0.5f;
        Exp = 2.8f;
        OctavesCount = 5;
        _GrassData = new ChunkGrassData(0, 0.6f);
    }

    public override void GenerateBiomeObjectsForChunk(Chunk chunk)
    {
        throw new System.NotImplementedException();
    }
}
