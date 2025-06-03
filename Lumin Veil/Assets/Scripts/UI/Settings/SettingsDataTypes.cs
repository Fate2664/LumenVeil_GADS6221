using UnityEngine;

[System.Serializable]
public abstract class Setting
{
    public string Name;
}

[System.Serializable]
public class BoolSetting : Setting  
{
    public bool State;
}
