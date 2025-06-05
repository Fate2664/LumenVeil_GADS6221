using Nova;
using NovaSamples.UIControls;
using System;
using System.ComponentModel;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public UIBlock Root = null;

    [Header("Temporary")]
    public BoolSetting BoolSetting = new BoolSetting();
    public ItemView ToggleItemView = null;
    public FloatSetting FloatSetting = new FloatSetting();
    public ItemView SliderItemView = null;
    public MultiOptionSetting MultiOptionSetting = new MultiOptionSetting();
    public ItemView DropDownItemView = null;

    private void Start()
    {
        Root.AddGestureHandler<Gesture.OnHover, ToggleVisuals>(ToggleVisuals.HandleHover);
        Root.AddGestureHandler<Gesture.OnUnhover, ToggleVisuals>(ToggleVisuals.HandleUnhover);
        Root.AddGestureHandler<Gesture.OnPress, ToggleVisuals>(ToggleVisuals.HandlePress);
        Root.AddGestureHandler<Gesture.OnRelease, ToggleVisuals>(ToggleVisuals.HandleRelease);

        Root.AddGestureHandler<Gesture.OnHover, DropDownVisuals>(DropDownVisuals.HandleHover);
        Root.AddGestureHandler<Gesture.OnUnhover, DropDownVisuals>(DropDownVisuals.HandleUnhover);
        Root.AddGestureHandler<Gesture.OnPress, DropDownVisuals>(DropDownVisuals.HandlePress);
        Root.AddGestureHandler<Gesture.OnRelease, DropDownVisuals>(DropDownVisuals.HandleRelease);

        Root.AddGestureHandler<Gesture.OnClick, ToggleVisuals>(HandleToggleClick);
        Root.AddGestureHandler<Gesture.OnDrag, SliderVisuals>(HandleSliderDragged);
        Root.AddGestureHandler<Gesture.OnClick, DropDownVisuals>(HandleDropDownClick);
        //Temporary
        BindToggle(BoolSetting, ToggleItemView.Visuals as ToggleVisuals);
        BindSlider(FloatSetting, SliderItemView.Visuals as SliderVisuals);
        BindDropDown(MultiOptionSetting, DropDownItemView.Visuals as DropDownVisuals);
    }

    private void HandleDropDownClick(Gesture.OnClick evt, DropDownVisuals target)
    {
        if (target.isExpanded)
        {
            target.Collapse();
        }
        else
        {
            target.Expand(MultiOptionSetting);
        }
    }

    private void HandleSliderDragged(Gesture.OnDrag evt, SliderVisuals target)
    {
        Vector3 localPointerPos = target.SliderBackground.transform.InverseTransformPoint(evt.PointerPositions.Current);

        float sliderWidth = target.SliderBackground.CalculatedSize.X.Value;

        float distanceFromLeft = Mathf.Clamp(localPointerPos.x + (sliderWidth * 0.5f), 0f, sliderWidth);

        float percentFromLeft = distanceFromLeft / sliderWidth;

        FloatSetting.value = Mathf.Lerp(FloatSetting.Min, FloatSetting.Max, percentFromLeft);

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

    private void BindDropDown(MultiOptionSetting setting, DropDownVisuals visuals)
    {
        visuals.label.Text = setting.Name;
        visuals.SelectedLabel.Text = setting.CurrentSelection;
        visuals.Collapse();
    }
}
