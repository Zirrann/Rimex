using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using static Properties;

public class ChunkManager : MonoBehaviour
{
    public Transform player;
    
    private float checkInterval = 1f;
    private HashSet<Vector2Int> currentActiveChunks = new HashSet<Vector2Int>();
    private HashSet<Vector2Int> chunksToDeactivate = new HashSet<Vector2Int>();

    void Start()
    {
        foreach (var chunk in TerrainGenerator.terrainChunks.Values) 
        { 
            chunk.DeactivateAllObjects();
        }
        StartCoroutine(MenageChunksState());
    }


    IEnumerator MenageChunksState()
    {
        while (true)
        { 
            Vector2 position = new Vector2(player.position.x, player.position.z);

            position += (new Vector2(worldSize, worldSize)) / 2;
            position /= chunkSize;

            chunksToDeactivate = new HashSet<Vector2Int>(currentActiveChunks);

            Vector2Int playerPosition = new Vector2Int(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y));
            Chunk chunk;

            for (int y = -renderDistance / 2; y < renderDistance / 2; y++) 
            {
                for (int x = -renderDistance / 2; x < renderDistance / 2; x++) 
                { 
                    Vector2Int chunkPosition = new Vector2Int(x,y) + playerPosition;

                    // skip chunks that are already active
                    if (currentActiveChunks.Contains(chunkPosition)) 
                    {
                        chunksToDeactivate.Remove(chunkPosition);                        
                    }

                    if (TerrainGenerator.terrainChunks.TryGetValue(chunkPosition, out chunk))
                    {
                        chunk.ActivateAllObjects();
                        currentActiveChunks.Add(chunkPosition);
                    }
                }
            }

            DeactivateChunks();

            yield return new WaitForSeconds(checkInterval);
        }
    }

    private void DeactivateChunks() 
    {
        foreach (var chunkPosition in chunksToDeactivate) 
        {
            TerrainGenerator.terrainChunks[chunkPosition].DeactivateAllObjects();
        }
    }
}
