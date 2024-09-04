using UnityEngine;

[CreateAssetMenu(fileName = "Properties", menuName = "ScriptableObjects/Properties", order = 1)]
public class PropertiesData : ScriptableObject
{
    [Header("Worlds size options")]
    public int chunkSize = 50;
    public int worldSize = 400;
    public int renderDistance = 10;

    [Header("Generation values")]
    [Range(1, 10)] public byte octaves = 1;
    [Range(0, 3)] public float exp = 1;
    public float firstOctaceValue = 1;
    public float frequencyScale = 2;
    public float heightScale = 6f;
}