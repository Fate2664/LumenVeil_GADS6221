using Nova;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ToggleVisuals : ItemVisuals
{
    public TextBlock label = null;
    public UIBlock2D CheckBox = null;
    public UIBlock2D CheckMark = null;

    public Color DefaultColor;
    public Color HoveredColor;
    public Color PressedColor;

    public bool IsChecked
    {
        get => CheckMark.gameObject.activeSelf;
        set => CheckMark.gameObject.SetActive(value);
    }

  
    internal static void HandleHovers(Gesture.OnHover evt, ToggleVisuals target)
    {
        target.CheckBox.Color = target.HoveredColor;
    }

    internal static void HandleUnhovers(Gesture.OnHover evt, ToggleVisuals target)
    {
        target.CheckBox.Color = target.DefaultColor;
    }

    internal static void HandlePresses(Gesture.OnHover evt, ToggleVisuals target)
    {
        target.CheckBox.Color = target.PressedColor;
    }

    internal static void HandleReleases(Gesture.OnHover evt, ToggleVisuals target)
    {
        target.CheckBox.Color = target.HoveredColor;
    }
}
