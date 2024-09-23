using System.Collections.Generic;
using UnityEngine;
using static Properties;

public class Chunk
{
    public Vector2Int position;
    public Mesh terrain;

    private GameObject chunkParent = GameObject.Find("Chunks");
    public GameObject chunkObject;
    public BiomeType biomeType;
    public List<GameObject[]> chunkObjects;

    public Chunk(Vector2Int chunkCoord, Material material, Mesh terrain, BiomeType biomeType)
    {
        chunkObjects = new List<GameObject[]>();

        this.position = chunkCoord;
        this.terrain = terrain;
        this.biomeType = biomeType;

        Initialize(material);
        
        Biomes.Instance.GenerateChunkObjects(this, biomeType);
    }

    private void Initialize(Material material)
    {
        chunkObject = new GameObject($"Chunk_{position.x}_{position.y}");
        chunkObject.tag = "Chunk";
        chunkObject.transform.position = new Vector3(position.x * chunkSize - worldSize / 2, 0, position.y * chunkSize - worldSize / 2);

        if (material == null)
        {
            material = new Material(Shader.Find("Standard"));
        }

        chunkObject.AddComponent<MeshFilter>().mesh = terrain;
        chunkObject.AddComponent<MeshRenderer>().material = material;
        chunkObject.AddComponent<MeshCollider>();
        chunkObject.transform.parent = chunkParent.transform;

        ChunkComponent chunkComponent = chunkObject.AddComponent<ChunkComponent>();
        chunkComponent.biomeType = biomeType;
    }

    public void DeactivateAllObjects()
    {
        if (chunkObjects == null || chunkObjects.Count == 0)
        {
            Debug.LogWarning("chunkObjects list is empty. No objects to deactivate.");
            return;
        }

        foreach (var objectArray in chunkObjects)
        {
            if (objectArray == null)
            {
                Debug.LogWarning("One of the arrays in chunkObjects is null.");
                continue;
            }

            foreach (var gameObject in objectArray)
            {
                if (gameObject != null)
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    Debug.LogWarning("Found a null object in the array.");
                }
            }
        }
    }

    public void ActivateAllObjects()
    {
        if (chunkObjects == null || chunkObjects.Count == 0)
        {
            Debug.LogWarning("chunkObjects list is empty. No objects to activate.");
            return;
        }

        foreach (var objectArray in chunkObjects)
        {
            if (objectArray == null)
            {
                Debug.LogWarning("One of the arrays in chunkObjects is null.");
                continue;
            }

            foreach (var gameObject in objectArray)
            {
                if (gameObject != null)
                {
                    gameObject.SetActive(true);
                }
                else
                {
                    Debug.LogWarning("Found a null object in the array.");
                }
            }
        }
    }
}
