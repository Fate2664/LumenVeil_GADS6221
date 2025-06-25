using Nova;
using UnityEngine;

[System.Serializable]
public class InventoryItemVisuals : ItemVisuals
{
    public UIBlock contentRoot;
    public UIBlock2D Image;
    public UIBlock2D CountBarFill;
    public TextBlock Count;

    [Header("Animations")]
    public float duration = .15f;
    public BodyColorAnimation hoverAnimation;
    public BodyColorAnimation unhoverAnimation;
    public GradientColorAnimation pressAnimation;
    public GradientColorAnimation releaseAnimation;

    private AnimationHandle hoverHandle;
    private AnimationHandle pressHandle;

    public void Hover()
    {
        hoverHandle.Cancel();
        hoverHandle = hoverAnimation.Run(duration);
    }

    public void Unhover()
    {
        hoverHandle.Cancel();
        hoverHandle = unhoverAnimation.Run(duration);
    }

    public void Press()
    {
        pressHandle.Cancel();
        pressHandle = pressAnimation.Run(duration);
    }

    public void Release()
    {
        pressHandle.Cancel();
        pressHandle = releaseAnimation.Run(duration);
    }

    public void Bind(InventoryItem data)
    {
        if (data.isEmpty)
        {
            contentRoot.gameObject.SetActive(false);
        }
        else
        {
            contentRoot.gameObject.SetActive(true);
            Image.SetImage(data.item.Icon);
            Count.Text = data.count.ToString();
            CountBarFill.Size.X.Percent = Mathf.Clamp01((float)data.count / InventoryItem.maxCount);
        }
    }
}
