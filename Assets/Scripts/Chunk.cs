using UnityEngine;
using static Properties;

public class Chunk
{
    public Vector2Int position;
    public Mesh terrain;

    private GameObject chunkParent = GameObject.Find("Chunks");
    public GameObject chunkObject;

    public Chunk(Vector2Int chunkCoord)
    {
        this.position = chunkCoord;
        terrain = TerrainGenerator.GenerateChunk(position);

        chunkObject = new GameObject($"Chunk_{chunkCoord.x}_{chunkCoord.y}");
        chunkObject.tag = "Chunk";
        chunkObject.transform.position = new Vector3(chunkCoord.x * chunkSize - worldSize / 2, 0, chunkCoord.y * chunkSize - worldSize / 2);

        chunkObject.AddComponent<MeshFilter>().mesh = terrain;
        chunkObject.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
        chunkObject.AddComponent<MeshCollider>();
        chunkObject.transform.parent = chunkParent.transform;
    }
}