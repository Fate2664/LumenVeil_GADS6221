using UnityEngine;
using NovaSamples.UIControls;

public class UIAudioHook : MonoBehaviour
{

    private Button novaButton;
    private UnityEngine.UI.Button unityButton;
    private void Awake()
    {
        novaButton = GetComponent<NovaSamples.UIControls.Button>();
        unityButton = GetComponent<UnityEngine.UI.Button>();
        if (novaButton != null)
        {
            novaButton.OnClicked.AddListener(() => AudioManager.Instance?.PlaySFX("ClickSound"));
            novaButton.OnHover.AddListener(() => AudioManager.Instance?.PlaySFX("HoverSound"));
        }

        if (unityButton != null)
        {
            unityButton.onClick.AddListener(() => AudioManager.Instance?.PlaySFX("ClickSound"));
        }
    }

}
