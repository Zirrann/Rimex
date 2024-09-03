using UnityEngine;
using static Properties;

public class Chunk
{
    public Vector2Int position;
    public Mesh terrain;

    public GameObject chunkObject = GameObject.Find("Chunks");

    public Chunk(Vector2Int chunkCoord)
    {
        this.position = chunkCoord;
        terrain = TerrainGenerator.GenerateChunk(position);
        chunkObject = new GameObject($"Chunk_{chunkCoord.x}_{chunkCoord.y}");
        chunkObject.transform.position = new Vector3(chunkCoord.x * chunkSize, 0, chunkCoord.y * chunkSize);
        chunkObject.AddComponent<MeshFilter>().mesh = terrain;
        chunkObject.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
        chunkObject.AddComponent<MeshCollider>();
    }
}