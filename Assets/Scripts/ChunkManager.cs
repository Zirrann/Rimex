using System.Collections.Generic;
using UnityEngine;
using static Properties;
public class ChunkManager : MonoBehaviour
{
    public Transform player;

    private float checkInterval = 1f;

    private Dictionary<Vector2Int, Chunk> loadedChunks = new Dictionary<Vector2Int, Chunk>();
    private Dictionary<Vector2Int, Chunk> unloadedChunks = new Dictionary<Vector2Int, Chunk>();


    void Update()
    {
        LoadAndUnloadChunks();
    }

    void LoadAndUnloadChunks()
    {
        Vector2Int playerChunkCoord = new Vector2Int(
            Mathf.FloorToInt(player.position.x / chunkSize),
            Mathf.FloorToInt(player.position.z / chunkSize)
        );

        for (int xOffset = -renderDistance; xOffset <= renderDistance; xOffset++)
        {
            for (int zOffset = -renderDistance; zOffset <= renderDistance; zOffset++)
            {
                Vector2Int chunkCoord = new Vector2Int(playerChunkCoord.x + xOffset, playerChunkCoord.y + zOffset);

                if (!loadedChunks.ContainsKey(chunkCoord))
                {
                    LoadChunk(chunkCoord);
                }
            }
        }

        List<Vector2Int> chunksToUnload = new List<Vector2Int>();
        foreach (var chunk in loadedChunks)
        {
            if (Vector2Int.Distance(playerChunkCoord, chunk.Key) > renderDistance)
            {
                chunksToUnload.Add(chunk.Key);
            }
        }

        foreach (var chunkCoord in chunksToUnload)
        {
            UnloadChunk(chunkCoord);
        }
    }

    void LoadChunk(Vector2Int coord)
    {
        Chunk chunk;

        if (unloadedChunks.TryGetValue(coord, out chunk))
        {
            GameObject chunkObject = GameObject.Find($"Chunk_{coord.x}_{coord.y}");

            if (chunkObject != null)
            {
                chunkObject.SetActive(true);
            }
        }
        else
        {
            chunk = new Chunk(coord);
        }

        loadedChunks[coord] = chunk;
        unloadedChunks.Remove(coord);
    }

    void UnloadChunk(Vector2Int coord)
    {
        if (loadedChunks.ContainsKey(coord))
        {
            Chunk chunk = loadedChunks[coord];
            GameObject chunkObject = GameObject.Find($"Chunk_{coord.x}_{coord.y}");

            if (chunkObject != null)
            {
                chunkObject.SetActive(false);
            }

            unloadedChunks[coord] = chunk;
            loadedChunks.Remove(coord);
        }
    }
}
