using Nova;
using UnityEngine;

public class HealthBarView : MonoBehaviour
{
    [Header("Nova UI References")]
    public UIBlock2D BackgroundBar;
    public UIBlock2D FillBar;
    public UIBlock2D HeartIcon;

    [Header("Health Settings")]
    [Range(0, 100)]
    public float currentHealth = 100f;
    public float maxHealth = 100f;

    [Header("Fill Settings")]
    public float maxFillWidth = 200f;

    private void Update()
    {
        UpdateFill();
    }

    public void SetHealth(float current, float max)
    {
        currentHealth = Mathf.Clamp(current, 0, max);
        maxHealth = max;
        UpdateFill();
    }

    private void UpdateFill()
    {
        float percent = Mathf.Clamp01(currentHealth / maxHealth);
        FillBar.Size.X.Value = percent * maxFillWidth;
    }
}
