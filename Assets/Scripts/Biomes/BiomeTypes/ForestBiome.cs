
public class ForestBiome : Biome
{
    private TreeFactory treeFactory;
    private int minTreePerChunk = 1;
    private int maxTreePerChunk = 4;


    public ForestBiome() : base(0, 0, 0, 0, 0)
    {
        HeightScale = 10f;
        FrequencyScale = 2f;
        FirstOctaveValue = 0.1f;
        Exp = 0.9f;
        OctavesCount = 3;
        _GrassData = new ChunkGrassData(0, 0.02f);

        treeFactory = new TreeFactory(minTreePerChunk, maxTreePerChunk);
    }

    public override void GenerateBiomeObjectsForChunk(Chunk chunk)
    {
       treeFactory.GenerateTreesForChunk(chunk);
    }
}
