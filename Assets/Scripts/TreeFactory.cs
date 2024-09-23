using UnityEngine;
using static Properties;

public class TreeFactory
{
    private GameObject treePrefab;
    private int minTreeNum;
    private int maxTreeNum;

    private string treePrefabPath = "Biomes/Trees/BasicTreeFilerPrefab";

    public TreeFactory( int minTreeNum, int maxTreeNum)
    {
        treePrefab = Resources.Load<GameObject>(treePrefabPath);

        this.minTreeNum = minTreeNum;
        this.maxTreeNum = maxTreeNum;
    }

    public void GenerateTreesForChunk(Chunk chunk)
    {
        int treeCount = Random.Range(minTreeNum, maxTreeNum + 1);
        GameObject[] trees = new GameObject[treeCount];

        for (int i = 0; i < treeCount; i++)
        {
            int x = Random.Range(0, chunkSize);
            int y = Random.Range(0, chunkSize);

            float terrainHeight = chunk.terrain.vertices[y * (chunkSize + 1) + x].y;
            Vector3 treePosition = new Vector3(chunk.position.x * chunkSize - worldSize / 2 + x, terrainHeight, chunk.position.y * chunkSize - worldSize / 2  + y);

            GameObject tree = GameObject.Instantiate(treePrefab);
            tree.transform.position = treePosition;

            trees[i] = tree;
        }

        chunk.chunkObjects.Add(trees);
    }
}
