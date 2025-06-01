using Nova;
using System;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public UIBlock Root = null;

    [Header("Temporary")]
    public BoolSetting BoolSetting = new BoolSetting();
    public ItemView toggleItemView = null;

    private void Start()
    {
        //Visual only
        //Root.AddGestureHandler<Gesture.OnHover, ToggleVisuals>(ToggleVisuals.HandleHovers);
        //Root.AddGestureHandler<Gesture.OnUnhover, ToggleVisuals>(ToggleVisuals.HandleUnhovers);
        //Root.AddGestureHandler<Gesture.OnPress, ToggleVisuals>(ToggleVisuals.HandlePresses);
        //Root.AddGestureHandler<Gesture.OnRelease, ToggleVisuals>(ToggleVisuals.HandleReleases);

        //State Change
        Root.AddGestureHandler<Gesture.OnClick, ToggleVisuals>(HandleToggleClicked);

        //Temporary
        BindToggle(BoolSetting, toggleItemView.Visuals as ToggleVisuals);
    }

    private void HandleToggleClicked(Gesture.OnClick evt, ToggleVisuals target)
    {
        BoolSetting.State = !BoolSetting.State;
        target.IsChecked = BoolSetting.State;
    }

    private void BindToggle(BoolSetting boolSetting, ToggleVisuals visuals)
    {
        visuals.label.Text = boolSetting.Name;
        visuals.IsChecked = boolSetting.State;
    }
}

