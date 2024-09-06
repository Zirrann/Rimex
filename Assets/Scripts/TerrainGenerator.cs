 using UnityEngine;
using static Properties;

using System.Collections.Generic;
using System;
using System.Linq;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TerrainGenerator : MonoBehaviour
{
    public Dictionary<Vector2Int, Chunk> terrainChunks = new Dictionary<Vector2Int, Chunk>();

    public Material grassMaterial;


    void Start()
    {
        GenerateTerrain();
    }

    void GenerateTerrain()
    {
        int numChunks = worldSize / chunkSize;

        for (int x = 0; x < numChunks; x++)
        {
            for (int z = 0; z < numChunks; z++)
            {
                Vector2Int chunkCoord = new Vector2Int(x, z);
                Mesh chunkMesh = GenerateChunk(chunkCoord);
                chunkMesh.RecalculateNormals();
                terrainChunks.Add(chunkCoord, new Chunk(chunkCoord, grassMaterial, chunkMesh));
            }
        }

        var keys = terrainChunks.Keys.ToList();
        foreach (var key in keys)
        {
            terrainChunks[key] = AdjustChunkToNeighbours(terrainChunks[key]);
        }
    }

    public static Mesh GenerateChunk(Vector2 chunkCoord)
    {
        Vector3[] vertices = new Vector3[(chunkSize + 1) * (chunkSize + 1)];
        int[] triangles = new int[chunkSize * chunkSize * 6];
        Vector2[] uvs = new Vector2[vertices.Length];

        (float firstOctaceValue, int octavesCount, float frequencyScale, float heightScale, float exp) = GetBiomeParams(chunkCoord.x, chunkCoord.y);

        for (int i = 0, z = 0; z <= chunkSize; z++)
        {
            for (int x = 0; x <= chunkSize; x++, i++)
            {

                float y = GetNoiceValue(x + chunkCoord.x * chunkSize, z + chunkCoord.y * chunkSize,
                    firstOctaceValue, octavesCount, frequencyScale, heightScale, exp);           
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

        return mesh;
    }


    private static float GetNoiceValue(float x, float y, float firstOctaceValue, int octavesCount, float frequencyScale, float heightScale, float exp ) {

        x = x / worldSize;
        y = y / worldSize;

        float noice = 0;
        float ocatave = firstOctaceValue;
        float n = 0;

        for (int i = 0; i < octavesCount; i++)
        {
            noice += Mathf.PerlinNoise(x * frequencyScale * ocatave, y * frequencyScale * ocatave)  / ocatave;
            n += 1 / ocatave;
            ocatave *= 2;

        }

        noice = noice / n;
        noice = (float)Math.Pow(noice, exp) - 0.5f;

        return noice * heightScale;
    }


    private static float GetBiome(float x, float y, int seed)
    {
        float biomeFrequency = 1.5f;
       // x = (x + (seed & worldSize) / worldSize); 
       // y = (y + (seed & worldSize) / worldSize);   
        x = x / worldSize * chunkSize;
        y = y / worldSize * chunkSize;

        float  b = Mathf.PerlinNoise((x) * biomeFrequency, (y) * biomeFrequency);
        
        return b;
    }

    private static (float, int, float, float, float) GetBiomeParams(float x, float y) 
    {

        float biomeValue = GetBiome(x, y, 47231);


        float firstOctaceValue;
        float frequencyScale;
        float heightScale;
        float exp;
        int octavesCount;



        if (biomeValue < 0.3f)
        {
            heightScale = 10f;
            frequencyScale = 3f;
            firstOctaceValue = 2f;
            octavesCount = 1;
            exp = 0.8f;
        }
        else if (biomeValue < 0.4f)
        {
            heightScale = 12f;
            frequencyScale = 2f;
            firstOctaceValue = 1f;
            octavesCount = 2;
            exp = 0.8f;
        }
        else if (biomeValue < 0.5f)
        {
            heightScale = 16f;
            frequencyScale = 2f;
            firstOctaceValue = 2.5f;
            octavesCount = 2;
            exp = 0.7f;
        }
        else if (biomeValue < 0.6f)
        {
            heightScale = 30f;
            frequencyScale = 2f;
            firstOctaceValue = 3f;
            octavesCount = 3;
            exp = 0.8f;
        }
        else if (biomeValue < 0.7f)
        {
            heightScale = 40f;
            frequencyScale = 4f;
            firstOctaceValue = 2f;
            octavesCount = 4;
            exp = 1f;
        }
        else
        {
            heightScale = 50f;
            frequencyScale = 4f;
            firstOctaceValue = 3f;
            octavesCount = 5;
            exp = 1.2f;
        }

        return (firstOctaceValue, octavesCount, frequencyScale, heightScale, exp);
    }


    Chunk AdjustChunkToNeighbours(Chunk chunk)
    {
        Vector3[] vertices = chunk.terrain.vertices;
        Vector2Int chunkCoord = chunk.position;

        
        if (terrainChunks.TryGetValue(new Vector2Int(chunkCoord.x + 1, chunkCoord.y), out Chunk rightChunk))
        {
            Vector3[] neighborVertices = rightChunk.terrain.vertices;
            for (int i = 0; i < chunkSize; i++)
            {
                int j = i * (chunkSize + 1) + chunkSize;
                vertices[j].y = neighborVertices[i * (chunkSize + 1)].y;
            }
        }

        if (terrainChunks.TryGetValue(new Vector2Int(chunkCoord.x, chunkCoord.y + 1), out Chunk aboveChunk))
        {
            Vector3[] neighborVertices = aboveChunk.terrain.vertices;
            for (int i = 0; i <= chunkSize; i++)
            {
                int j = chunkSize * (chunkSize + 1) + i;
                vertices[j].y = neighborVertices[i].y;
            }
        }

        if (terrainChunks.TryGetValue(new Vector2Int(chunkCoord.x + 1, chunkCoord.y + 1), out Chunk cornerChunk))
        {
            Vector3[] neighborVertices = cornerChunk.terrain.vertices;
            int j = vertices.Length - 1;
            vertices[j].y = neighborVertices[0].y;
        }

        vertices = AverageSmoothing(vertices);

        chunk.terrain.vertices = vertices;
       
        return chunk;
    }

    Vector3[] AverageSmoothing(Vector3[] vertices)
    {
        int topBottomSmoothRange = chunkSize/ 2;

        for (int i = 0; i <= chunkSize; i++)
        {
            for (int j = 0; j <= topBottomSmoothRange; j++)
            {
                float ratio = (float)(topBottomSmoothRange - j) / topBottomSmoothRange;
                int index = (chunkSize - j) * (chunkSize + 1) + i;
                vertices[index].y = vertices[(chunkSize) * (chunkSize + 1) + i].y * ratio + vertices[index].y * (1 - ratio);
            }
        }

        int leftRightSmoothRange = chunkSize / 3;

        for (int i = 1; i <= chunkSize +1; i++)
        {
            for (int j = 0; j <= leftRightSmoothRange; j++)
            {
                float ratio = (float)(leftRightSmoothRange - j) / leftRightSmoothRange;
                int index = i * (chunkSize + 1) - j - 1;
                vertices[index].y = vertices[i * (chunkSize + 1) - 1].y * ratio + vertices[index].y * (1 - ratio);
            }
        }


        return vertices;
    }
}