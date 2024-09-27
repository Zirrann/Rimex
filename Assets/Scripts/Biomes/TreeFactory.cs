using System.Collections.Generic;
using UnityEngine;
using static Properties;

public class TreeFactory
{
    private GameObject treePrefab;
    private int minTreeNum;
    private int maxTreeNum;

    private string treePrefabPath = "Biomes/Trees/BasicTreeFilerPrefab";

    public TreeFactory( int minTreeNum, int maxTreeNum)
    {
        treePrefab = Resources.Load<GameObject>(treePrefabPath);

        this.minTreeNum = minTreeNum;
        this.maxTreeNum = maxTreeNum;
    }

    public void GenerateTreesForChunk(Chunk chunk)
    {
        int treeCount = Random.Range(minTreeNum, maxTreeNum + 1);
        GameObject[] trees = new GameObject[treeCount];

        float minDistance = 5f;
        List<Vector3> placedTreePositions = new List<Vector3>();

        for (int i = 0; i < treeCount; i++)
        {
            Vector3 treePosition;
            bool validPosition = false;
            int x, y;
            int attempts = 0;
            do
            {
                attempts++;
                x = Random.Range(0, chunkSize);
                y = Random.Range(0, chunkSize);

                float terrainHeight = chunk.terrain.vertices[y * (chunkSize + 1) + x].y;
                treePosition = new Vector3(chunk.position.x * chunkSize - worldSize / 2 + x, terrainHeight, chunk.position.y * chunkSize - worldSize / 2 + y);

                validPosition = true;

                foreach (Vector3 placedPosition in placedTreePositions)
                {
                    if (Vector3.Distance(treePosition, placedPosition) < minDistance)
                    {
                        validPosition = false;
                        break;
                    }
                }

                if (attempts > 10)
                {
                    break;
                }

            } while (!validPosition);

            if (!validPosition)
            {
                continue;
            }

            Vector3 terrainNormal = chunk.terrain.normals[y * (chunkSize + 1) + x];

            GameObject tree = GameObject.Instantiate(treePrefab);
            tree.transform.position = treePosition;

            float tiltX = Mathf.Asin(terrainNormal.z) * Mathf.Rad2Deg;
            float tiltZ = Mathf.Asin(terrainNormal.x) * Mathf.Rad2Deg;

            tiltX = tiltX / 90 + Random.Range(-5f, 5f);
            tiltZ = tiltZ / 90 + Random.Range(-5f, 5f);

            tree.transform.rotation = Quaternion.Euler(tiltX, 0f, tiltZ);

            placedTreePositions.Add(treePosition);
            trees[i] = tree;
        }

        chunk.chunkObjects.Add(trees);
    }
}
