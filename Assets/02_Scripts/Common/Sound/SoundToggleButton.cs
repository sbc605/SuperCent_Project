using UnityEngine;
using UnityEngine.UI;

public class SoundToggleButton : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private Button button;
    [SerializeField] private Image buttonImage;

    [Header("Colors")]
    [SerializeField] private Color soundOnColor = Color.white;
    [SerializeField] private Color soundOffColor = Color.gray;

    [Header("Optional SFX")]
    [SerializeField] private string clickSoundName = "setting_click";

    private void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();

        if (buttonImage == null)
            buttonImage = GetComponent<Image>();

        if (button != null)
            button.onClick.AddListener(OnClickSoundButton);
    }

    private void Start()
    {
        RefreshButtonColor();
    }

    private void OnDestroy()
    {
        if (button != null)
            button.onClick.RemoveListener(OnClickSoundButton);
    }

    private void OnClickSoundButton()
    {
        if (SoundManager.Instance == null)
        {
            Debug.LogWarning("[SoundToggleButton] SoundManager가 씬에 없습니다.");
            return;
        }

        // 현재 켜져 있는 상태에서 누르면 클릭음이 한 번 들리고 꺼짐
        if (!string.IsNullOrEmpty(clickSoundName))
        {
            SoundManager.Instance.UISoundPlay(clickSoundName);
        }

        SoundManager.Instance.ToggleMute();
        RefreshButtonColor();
    }

    private void RefreshButtonColor()
    {
        if (buttonImage == null)
            return;

        if (SoundManager.Instance == null)
            return;

        buttonImage.color = SoundManager.Instance.IsMuted ? soundOffColor : soundOnColor;
    }
}