using UnityEngine;
using static Properties;

using System.Collections.Generic;
using System;

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
        Debug.Log(chunkSize);
        int numChunks = worldSize / chunkSize;

        for (int x = 0; x < numChunks; x++)
        {
            for (int z = 0; z < numChunks; z++)
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

                float y = GetNoiceValue(x + chunkCoord.x * chunkSize, z + chunkCoord.y * chunkSize);
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


    private static float GetNoiceValue(float x, float y) {

        x = x / worldSize;
        y = y / worldSize;

        float noice = 0;
        float ocatave = firstOctaceValue;
        float n = 0;

        for (int i = 0; i < octaves; i++) 
        {
            noice += Mathf.PerlinNoise(ocatave * x * frequencyScale, ocatave * y * frequencyScale) * heightScale / ocatave;
            n += 1 / ocatave;
            ocatave *= 2;
            
        }

        noice = noice / n;

        return (float)Math.Pow(noice, exp) - heightScale/2;
    }
}  