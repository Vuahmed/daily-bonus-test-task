using System.IO;
using UnityEngine;

public class ConfigLoader
{
    public static ConfigData Load()
    {
        TextAsset json = GetConfig();
        
        if (json == null)
        {
            Debug.LogError("Config not found!");
            return null;
        }

        ConfigData config = JsonUtility.FromJson<ConfigData>(json.text);
        Debug.Log("Type: " + config.DailyBonusType);
        
        return config;
    }

    private static TextAsset GetConfig()
    {
        return Resources.Load<TextAsset>("Config");
    }
    
}
