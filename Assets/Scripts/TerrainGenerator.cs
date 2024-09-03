using UnityEngine;
using static Properties;

using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TerrainGenerator : MonoBehaviour
{
    public Dictionary<Vector2Int, Chunk> terrainChunks = new Dictionary<Vector2Int, Chunk>();

    void Start()
    {
       GenerateTerrain();
    }

    void GenerateTerrain()
    {
        int numChunks = worldPreRenderSizie / chunkSize /2;

        for (int x = -numChunks; x < numChunks; x++)
        {
            for (int z = -numChunks; z < numChunks; z++)
            {
                Vector2Int chunkCoord = new Vector2Int(x, z);
                Mesh chunkMesh = GenerateChunk(chunkCoord);
                terrainChunks.Add(chunkCoord, new Chunk(chunkCoord));
            }
        }
    }

    public static Mesh GenerateChunk(Vector2 chunkCoord)
    {
        Vector3[] vertices = new Vector3[(chunkSize + 1) * (chunkSize + 1)];
        int[] triangles = new int[chunkSize * chunkSize * 6];
        Vector2[] uvs = new Vector2[vertices.Length];

        for (int i = 0, z = 0; z <= chunkSize; z++)
        {
            for (int x = 0; x <= chunkSize; x++, i++)
            {
                float y = Mathf.PerlinNoise((x + chunkCoord.x * chunkSize) / noiseScale, (z + chunkCoord.y * chunkSize) / noiseScale) * heightScale;
                vertices[i] = new Vector3(x, y, z);
                uvs[i] = new Vector2((float)x / chunkSize, (float)z / chunkSize);
            }
        }

        int vert = 0;
        int tris = 0;
        for (int z = 0; z < chunkSize; z++)
        {
            for (int x = 0; x < chunkSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + chunkSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + chunkSize + 1;
                triangles[tris + 5] = vert + chunkSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        return mesh;
    }
}  