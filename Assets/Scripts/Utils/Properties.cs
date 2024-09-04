using UnityEngine;
public static class Properties
{
    private static PropertiesData _instance;

    public static void Initialize(PropertiesData instance)
    {
        _instance = instance;
    }

    public static int chunkSize => _instance.chunkSize;
    public static int worldSize => _instance.worldSize;
    public static int renderDistance => _instance.renderDistance;



    public static float firstOctaceValue => _instance.firstOctaceValue;
    public static float exp => _instance.exp;
    public static ushort octaves => _instance.octaves;
    public static float frequencyScale => _instance.frequencyScale;

    public static float heightScale => _instance.heightScale;
}

