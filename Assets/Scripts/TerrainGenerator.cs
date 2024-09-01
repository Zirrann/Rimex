using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TerrainGenerator : MonoBehaviour
{
    public int xSize = 10;
    public int zSize = 10;
    public float noiseScale = 0.5f;
    public float quadSize = 10f;

    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    MeshCollider meshCollider;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        GenerateShape();
        UpdateMesh();

        Material material = GetComponent<Material>();
        if (material == null)
        {
             material = new Material(Shader.Find("Standard"));

        }

        Vector3 colorV = new Vector3(146f, 255f, 107f).normalized;
        material.color = new Color(colorV.x,colorV.y,colorV.z);
        GetComponent<MeshRenderer>().material = material;

        meshCollider = GetComponent<MeshCollider>();

        meshCollider.sharedMesh = mesh;
    }

    void GenerateShape()
    {

        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        triangles = new int[xSize * zSize * 6];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * noiseScale, z * noiseScale) * 3f;
                
                vertices[i++] = new Vector3(x * quadSize, y, z * quadSize);
            }
        }

        int vert = 0;
        int tris = 0;
        for (int x = 0; x < xSize; x++)
        {
            for (int z = 0; z < zSize; z++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;

                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }


}
