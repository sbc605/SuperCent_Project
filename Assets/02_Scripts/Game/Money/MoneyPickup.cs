using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 돈 프리팹에 붙여서 사용
/// 플레이어가 돈을 먹으면 인벤토리에 들어가는 기능
/// </summary>
public class MoneyPickup : MonoBehaviour
{
    [SerializeField] private int amount = 10;
    private WorkbenchStackView owner;

    public void SetAmount(int value)
    {
        amount = value;
    }

    public void SetOwner(WorkbenchStackView stackView)
    {
        owner = stackView;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory inventory = other.GetComponentInParent<PlayerInventory>();
        if (inventory == null)
            return;

        if (inventory.TryAddMoney(1)) // 묶음 1개
        {
            inventory.AddMoneyValue(amount); // 실제 돈 10

            if (owner != null)
                owner.RemoveItem(gameObject);
            else
                Destroy(gameObject);
        }
    }
}
