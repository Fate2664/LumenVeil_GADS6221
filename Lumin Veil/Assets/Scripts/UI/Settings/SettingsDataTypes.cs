using UnityEngine;

[System.Serializable]
public abstract class Setting
{
    public string Key;
    public string Name;

    public virtual void ResetToDefault() { }
}

[System.Serializable]
public class BoolSetting : Setting  
{
    public bool State;
    public bool DefaultValue = false;

    public void Save() => PlayerPrefs.SetInt(Key, State ? 1 : 0);
    public void Load() => State = PlayerPrefs.GetInt(Key, DefaultValue ? 1 : 0) == 1;

    public override void ResetToDefault() 
    {
        State = DefaultValue;
        Save();
    }
}

[System.Serializable]
public class FloatSetting : Setting
{
    [SerializeField]
    public float value;
    public float Min;
    public float Max;
    public string ValueFormat = "{0:0.0}";
    public float DefaultValue = 50f;

    public float Value
    {
        get => Mathf.Clamp(value, Min, Max);
        set => this.value = Mathf.Clamp(value, Min, Max);
    }

    public string DisplayValue => string.Format(ValueFormat, Value);

    public void Save() => PlayerPrefs.SetFloat(Key, Value);
    public void Load() => value = PlayerPrefs.GetFloat(Key, DefaultValue);

    public override void ResetToDefault()
    {
        value = DefaultValue;
        Save();
    }
}

[System.Serializable]
public class MultiOptionSetting : Setting
{
    private const string NothingSelected = "None";

    public string[] Options = new string[0];
    public int SelectedIndex = 0;
    public int DefaultIndex = 0;

    public string CurrentSelection => SelectedIndex >= 0 && SelectedIndex < Options.Length ? Options[SelectedIndex] : NothingSelected;

    public void Save() => PlayerPrefs.SetInt(Key, SelectedIndex);
    public void Load() => SelectedIndex = PlayerPrefs.GetInt(Key, DefaultIndex);

    public override void ResetToDefault()
    {
        SelectedIndex = DefaultIndex;
        Save();
    }
}