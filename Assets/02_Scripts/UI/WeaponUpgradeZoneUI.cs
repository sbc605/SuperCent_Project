using TMPro;
using UnityEngine;

public class WeaponUpgradeZoneUI : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private TMP_Text costText;

    public void Show(int remainCost)
    {
        if (root != null)
            root.SetActive(true);

        if (costText != null)
            costText.text = $"${remainCost}";
    }

    public void SetMax()
    {
        if (root != null)
            root.SetActive(true);

        if (costText != null)
            costText.text = "MAX";
    }

    public void Hide()
    {
        if (root != null)
            root.SetActive(false);
    }
}
