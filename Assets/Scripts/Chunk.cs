using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Chunk
{
    public Vector2Int position;
    public Mesh terrain;
    private TerrainGenerator generator = new TerrainGenerator();
    float chunkSize = 16f;
    public GameObject chunkObject = GameObject.Find("Chunks");

    public Chunk(Vector2Int chunkCoord)
    {
        this.position = chunkCoord;
        terrain = generator.GenerateChunk(position);
        chunkObject = new GameObject($"Chunk_{chunkCoord.x}_{chunkCoord.y}");
        chunkObject.transform.position = new Vector3(chunkCoord.x * chunkSize, 0, chunkCoord.y * chunkSize);
        chunkObject.AddComponent<MeshFilter>().mesh = terrain;
        chunkObject.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
        chunkObject.AddComponent<MeshCollider>();
    }
}