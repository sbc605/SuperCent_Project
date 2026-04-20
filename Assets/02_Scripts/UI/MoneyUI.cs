using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private TMP_Text moneyText;

    private void OnEnable()
    {
        if (inventory != null)
            inventory.OnMoneyValueChanged += RefreshUI;
    }

    private void OnDisable()
    {
        if (inventory != null)
            inventory.OnMoneyValueChanged -= RefreshUI;
    }

    private void Start()
    {
        if (inventory != null)
            RefreshUI(inventory.TotalMoney);
    }

    private void RefreshUI(int totalMoney)
    {
        if (moneyText != null)
            moneyText.text = $"${totalMoney}";
    }
}