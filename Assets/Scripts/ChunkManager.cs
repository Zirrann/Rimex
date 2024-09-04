using System.Collections;
using UnityEngine;
using static Properties;
public class ChunkManager : MonoBehaviour
{
    public Transform player;
    private GameObject[] chunks;

    private float checkInterval = 1f;

    void Start()
    {
        chunks = GameObject.FindGameObjectsWithTag("Chunk");
        //StartCoroutine(MenageChunksState());
    }

    IEnumerator MenageChunksState()
    {
        while (true)
        {

            foreach (GameObject chunk in chunks)
            {
                float distance = Vector2.Distance(
                    new Vector2 (chunk.transform.position.x, chunk.transform.position.z),
                    new Vector2(player.position.x, player.position.z));

                if (distance > renderDistance * chunkSize && chunk.activeSelf)
                {
                    chunk.SetActive(false);
                }
                else if (distance <= renderDistance * chunkSize && !chunk.activeSelf)
                {
                    chunk.SetActive(true);
                }
            }

            yield return new WaitForSeconds(checkInterval);
        }
    }
}
