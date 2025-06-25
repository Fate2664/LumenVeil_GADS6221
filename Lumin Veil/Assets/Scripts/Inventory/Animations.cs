using UnityEngine;
using Nova;

[System.Serializable]
public struct BodyColorAnimation : IAnimation
{
    public UIBlock Target;
    public Color TargetColor;

    private Color startColor;

    public void Update(float percentDone)
    {
        if (percentDone == 0f)
        {
            startColor = Target.Color;
        }

        Target.Color = Color.Lerp(startColor, TargetColor, percentDone);
    }
}

[System.Serializable]
public struct GradientColorAnimation : IAnimation
{
    public UIBlock2D Target;
    public Color TargetColor;

    private Color startColor;

    public void Update(float percentDone)
    {
        if (percentDone == 0f)
        {
            startColor = Target.Gradient.Color;
        }

        Target.Gradient.Color = Color.Lerp(startColor, TargetColor, percentDone);
    }
}