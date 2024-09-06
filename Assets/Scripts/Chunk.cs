using UnityEngine;
using static Properties;

public class Chunk
{
    public Vector2Int position;
    public Mesh terrain;

    private GameObject chunkParent = GameObject.Find("Chunks");
    public GameObject chunkObject;


    public Chunk(Vector2Int chunkCoord, Material material, Mesh terrain) 
    { 
        this.position = chunkCoord;
        this.terrain = terrain;

        Initialize(material);
    }

    private void Initialize(Material material) {
        chunkObject = new GameObject($"Chunk_{position.x}_{position.y}");
        chunkObject.tag = "Chunk";
        // chunkObject.transform.position = new Vector3(position.x * chunkSize - worldSize / 2, 0, position.y * chunkSize - worldSize / 2);
        chunkObject.transform.position = new Vector3(position.x * chunkSize , 0, position.y * chunkSize);
        if (material == null)
        {
            material = new Material(Shader.Find("Standard"));
        }

        chunkObject.AddComponent<MeshFilter>().mesh = terrain;
        chunkObject.AddComponent<MeshRenderer>().material = material;
        chunkObject.AddComponent<MeshCollider>();
        chunkObject.transform.parent = chunkParent.transform;
    }
}