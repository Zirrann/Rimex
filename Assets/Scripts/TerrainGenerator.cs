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
    public float[][] heightMap;
    public int gameSeed;
    
    private int seed;
    private Mesh baseMesh;
    private int numChunks;

    void OnEnable()
    {
        baseMesh = GenerateBazeMesh();
        seed = gameSeed % 99991;
    }

    public void GenerateTerrain()
    {
        if (baseMesh is null) 
        {
            baseMesh = GenerateBazeMesh();
        }

        numChunks = worldSize / chunkSize;

        heightMap = new float[numChunks * numChunks][];

        // Generate terrain heights base on noises
        for (int i = 0, x = 0; x < numChunks; x++)
        {
            for (int z = 0; z < numChunks; z++, i++)
            {
                heightMap[i] = GenerateChunkHeights(x, z);
            }
        }
        // Smoove heights changes on edges
        for (int i = 0, x = 0; x < numChunks; x++)
        {
            for (int z = 0; z < numChunks; z++, i++)
            {
                AdjustChunkToNeighbours(i);
            }
        }
        // Make chunks
        for (int i = 0, x = 0; x < numChunks; x++)
        {
            for (int z = 0; z < numChunks; z++, i++)
            {
                Vector2Int vec = new Vector2Int(x, z);
                Mesh mesh = CreateMesh(i);
                terrainChunks.Add(vec, new Chunk(vec, grassMaterial, mesh));
            }
        }
    }

    private Mesh CreateMesh(int index) {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[baseMesh.vertices.Length];

        for (int i = 0; i < heightMap[index].Length; i++) {
            vertices[i] = baseMesh.vertices[i] + new Vector3(0, heightMap[index][i],0);
        }

        mesh.vertices = vertices;
        mesh.uv = baseMesh.uv;
        mesh.triangles = baseMesh.triangles;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        return mesh;
    }

    public float[] GenerateChunkHeights(int chankX, int chankZ)
    {
        (float firstOctaceValue, int octavesCount, float frequencyScale, float heightScale, float exp) = GetBiomeParams(chankX, chankZ);

        float[] height = new float[(int)((chunkSize + 1) * (chunkSize + 1))];
        for (int i = 0, z = 0; z <= chunkSize; z++)
        {
            for (int x = 0; x <= chunkSize; x++, i++)
            {
                float y = GetNoiceValue(x + chankX * chunkSize, z + chankZ * chunkSize,
                    firstOctaceValue, octavesCount, frequencyScale, heightScale, exp);
                height[i] = y;
            }
        }

        return height;
    }

    private Mesh GenerateBazeMesh()
    {
        Vector3[] vertices = new Vector3[(chunkSize + 1) * (chunkSize + 1)];
        int[] triangles = new int[chunkSize * chunkSize * 6];
        Vector2[] uvs = new Vector2[vertices.Length];

        float[] height = new float[(int)((chunkSize + 1) * (chunkSize + 1))];
        for (int i = 0, z = 0; z <= chunkSize; z++)
        {
            for (int x = 0; x <= chunkSize; x++, i++)
            {
                vertices[i] = new Vector3(x, 0, z);
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

    private float GetNoiceValue(float x, float y, float firstOctaceValue, int octavesCount, float frequencyScale, float heightScale, float exp)
    {

        x = x / worldSize;
        y = y / worldSize;

        float noice = 0;
        float ocatave = firstOctaceValue;
        float n = 0;

        for (int i = 0; i < octavesCount; i++)
        {
            noice += Mathf.PerlinNoise(x * frequencyScale * ocatave, y * frequencyScale * ocatave) / ocatave;
            n += 1 / ocatave;
            ocatave *= 2;

        }

        noice = noice / n;
        noice = (float)Math.Pow(noice, exp);

        return noice * heightScale;
    }

    private (float, int, float, float, float) GetBiomeParams(float x, float y) 
    {
        float biomeValue = GetBiome(x, y);
        float upperBiomeValue = GetBiome(x, y+1);
        float rightBiomeValue = GetBiome(x+1, y);

        BiomeType biomeType = Biomes.GetBiomeType(biomeValue);
        BiomeType upperBiomeType = Biomes.GetBiomeType(upperBiomeValue);
        BiomeType rightBiomeType = Biomes.GetBiomeType(rightBiomeValue);
        Biome b;

        if (biomeType != upperBiomeType && biomeType != rightBiomeType) 
        {
            Biome biome = Biomes.Instance.GetBiome(biomeType);
            Biome upperBiome = Biomes.Instance.GetBiome(upperBiomeType);
            Biome rightBiome = Biomes.Instance.GetBiome(rightBiomeType);


            return ((biome.FirstOctaceValue + upperBiome.FirstOctaceValue + rightBiome.FirstOctaceValue) / 3,
                ((int)(biome.OctavesCount + upperBiome.OctavesCount + rightBiome.OctavesCount) / 3),
                (biome.FrequencyScale + upperBiome.FrequencyScale + rightBiome.FrequencyScale) / 3,
                (biome.HeightScale + upperBiome.HeightScale + rightBiome.HeightScale) / 3,
                (biome.Exp + upperBiome.Exp + rightBiome.Exp) / 3);
        }

        if (biomeType != upperBiomeType)
        {
            Biome biome = Biomes.Instance.GetBiome(biomeType);
            Biome upperBiome = Biomes.Instance.GetBiome(upperBiomeType);


            return ((biome.FirstOctaceValue + upperBiome.FirstOctaceValue) / 2,
                ((int)(biome.OctavesCount + upperBiome.OctavesCount) / 2),
                (biome.FrequencyScale + upperBiome.FrequencyScale) / 2,
                (biome.HeightScale + upperBiome.HeightScale) / 2,
                (biome.Exp + upperBiome.Exp) / 2);
        }

        if (biomeType != rightBiomeType)
        {
            Biome biome = Biomes.Instance.GetBiome(biomeType);
            Biome rightBiome = Biomes.Instance.GetBiome(rightBiomeType);

            return ((biome.FirstOctaceValue + rightBiome.FirstOctaceValue) / 2,
                ((int)(biome.OctavesCount +  rightBiome.OctavesCount) / 2),
                (biome.FrequencyScale +  rightBiome.FrequencyScale) / 2,
                (biome.HeightScale + rightBiome.HeightScale) / 2,
                (biome.Exp + rightBiome.Exp) / 2);
        }




        b = Biomes.Instance.GetBiome(biomeType);

        return (b.FirstOctaceValue, b.OctavesCount, b.FrequencyScale, b.HeightScale, b.Exp);
    }

    void AdjustChunkToNeighbours(int index)
    {
        // right edge
        if (index < numChunks * (numChunks - 1))
        {
            for (int i = 0; i <= chunkSize; i++)
            {
                int j = i * (chunkSize + 1) + chunkSize;
                heightMap[index][j] = heightMap[index + numChunks][i * (chunkSize + 1)];
            }
        }
        // top edge
        if ((index + 1 ) % numChunks != 0)
        {
            for (int i = 0; i <= chunkSize; i++)
            {
                int j = chunkSize * (chunkSize + 1) + i;
                heightMap[index][j] = heightMap[index + 1][i];
            }
        }
        // top-right conrer
        if (index + numChunks + 1 < heightMap.Length)
        {
            int j = heightMap[index].Length - 1;
            heightMap[index][j] = heightMap[index + numChunks + 1][0];
        }

         heightMap[index] = AverageSmoothing(heightMap[index]);
    }

    float Smoothstep(float edge0, float edge1, float x)
    {
        // Clamp x to the [0, 1] range
        x = Mathf.Clamp01((x - edge0) / (edge1 - edge0));
        return x * x * (3 - 2 * x);
    }

    float[] AverageSmoothing(float[] heights)
    {
        int topBottomSmoothRange = chunkSize / 2;

        for (int i = 0; i <= chunkSize; i++)
        {
            for (int j = 0; j <= topBottomSmoothRange; j++)
            {
                float ratio = (float)(topBottomSmoothRange - j) / topBottomSmoothRange;
                ratio = Smoothstep(0, 1, ratio);
                int index = (chunkSize - j) * (chunkSize + 1) + i;
                heights[index] = heights[chunkSize * (chunkSize + 1) + i] * ratio + heights[index] * (1 - ratio);
            }
        }

        int leftRightSmoothRange = chunkSize / 2;

        for (int i = 1; i <= chunkSize + 1; i++)
        {
            for (int j = 0; j <= leftRightSmoothRange; j++)
            {
                float ratio = (float)(leftRightSmoothRange - j) / leftRightSmoothRange;
                ratio = Smoothstep(0, 1, ratio);
                int index = i * (chunkSize + 1) - j - 1;
                heights[index] = heights[i * (chunkSize + 1) - 1] * ratio + heights[index] * (1 - ratio);
            }
        }

        return heights;
    }

    private float GetBiome(float x, float y)
    {
        float biomeFrequency = 1.5f + (seed % 10) * 0.1f;
        x = (x + (seed & worldSize) / worldSize); 
        y = (y + (seed & worldSize) / worldSize);

        x = x / worldSize * chunkSize;
        y = y / worldSize * chunkSize;


        float b = Mathf.PerlinNoise((x) * biomeFrequency, (y) * biomeFrequency);

        return b;
    }
}