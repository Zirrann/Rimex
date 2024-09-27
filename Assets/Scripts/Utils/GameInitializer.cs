using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    public PropertiesData propertiesData;

    void Awake()
    {
        if (propertiesData != null)
        {
            Properties.Initialize(propertiesData);
        }
        else
        {
            Debug.LogError("PropertiesData is not assigned in the Inspector!");
        }
    }
}
