using UnityEngine;

public class PlayerControlSettings
{
    // You can adjust this key to match your MultiOptionSetting key
    private const string Key = "ControlPreset";

    public static string CurrentPreset
    {
        get
        {
            int index = PlayerPrefs.GetInt(Key, 0);
            // You can adapt this array if your Options are different
            string[] presets = { "WASD", "ArrowKeys" };
            if (index < 0 || index >= presets.Length)
                return "WASD"; // fallback
            return presets[index];
        }
    }
}
