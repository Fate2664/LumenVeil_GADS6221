using Nova;
using UnityEngine;

[System.Serializable]
public class TabButtonVisuals : ItemVisuals
{
    public TextBlock label = null;
    public UIBlock2D background = null;
    public UIBlock2D selectedIndicator = null;

    public Color DefaultColor;
    public Color SelectedColor;

    public Color DefaultGradientColor;
    public Color HoveredGradientColor;
    public Color PressedGradientColor;

}
