using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 수감자 머리 위 주문 UI
/// 흰 배경 위에 초록 fill이 아래에서 위로 차오름
/// </summary>
public class PrisonerOrderUI : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private TMP_Text countText;
    [SerializeField] private Image fillImage; // Type = Filled, Fill Method = Vertical, Fill Origin = Bottom

    public void Show(int remainCount, int totalCount)
    {
        if (root != null)
            root.SetActive(true);

        if (countText != null)
            countText.text = remainCount.ToString();

        if (fillImage != null)
        {
            float progress = totalCount <= 0 ? 0f : 1f - (remainCount / (float)totalCount);
            fillImage.fillAmount = progress;
        }
    }

    public void Hide()
    {
        if (root != null)
            root.SetActive(false);
    }
}