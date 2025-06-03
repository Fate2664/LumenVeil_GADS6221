using Nova;
using NovaSamples.UIControls;
using System;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public UIBlock Root = null;

    [Header("Temporary")]
    public BoolSetting BoolSetting = new BoolSetting();
    public ItemView ToggleItemView = null;
    public FloatSetting FloatSetting = new FloatSetting();
    public ItemView SliderItemView = null;

    private void Start()
    {
        Root.AddGestureHandler<Gesture.OnHover, ToggleVisuals>(ToggleVisuals.HandleHover);
        Root.AddGestureHandler<Gesture.OnUnhover, ToggleVisuals>(ToggleVisuals.HandleUnhover);
        Root.AddGestureHandler<Gesture.OnPress, ToggleVisuals>(ToggleVisuals.HandlePress);
        Root.AddGestureHandler<Gesture.OnRelease, ToggleVisuals>(ToggleVisuals.HandleRelease);

        Root.AddGestureHandler<Gesture.OnClick, ToggleVisuals>(HandleToggleClick);
        Root.AddGestureHandler<Gesture.OnDrag, SliderVisuals>(HandleSliderDragged);
        //Temporary
        BindToggle(BoolSetting, ToggleItemView.Visuals as ToggleVisuals);
        BindSlider(FloatSetting, SliderItemView.Visuals as SliderVisuals);
    }

    private void HandleSliderDragged(Gesture.OnDrag evt, SliderVisuals target)
    {
        Vector3 currentPointerPos = evt.PointerPositions.Current;

        float localXPos = target.SliderBackground.transform.InverseTransformDirection(currentPointerPos).x;
        float sliderWidth = target.SliderBackground.CalculatedSize.X.Value;

        float distanceFromLeft = localXPos + .5f * sliderWidth;
        float percentFromLeft = Mathf.Clamp01(distanceFromLeft / sliderWidth);

        FloatSetting.value = FloatSetting.Min + percentFromLeft * (FloatSetting.Max - FloatSetting.Min);

        target.FillBar.Size.X.Percent = percentFromLeft;
        target.ValueLabel.Text = FloatSetting.DisplayValue;
    }

    private void HandleToggleClick(Gesture.OnClick evt, ToggleVisuals target)
    {
        BoolSetting.State = !BoolSetting.State;
        target.IsChecked = BoolSetting.State;
    }

    private void BindToggle(BoolSetting boolSetting, ToggleVisuals visuals)
    {
        visuals.label.Text = boolSetting.Name;
        visuals.IsChecked = boolSetting.State;
    }

    private void BindSlider(FloatSetting floatSetting, SliderVisuals visuals)
    {
        visuals.label.Text = floatSetting.Name;
        visuals.ValueLabel.Text = floatSetting.DisplayValue;
        visuals.FillBar.Size.X.Percent = (floatSetting.value - floatSetting.Min) / (floatSetting.Max - floatSetting.Min);
    }
}
