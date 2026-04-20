using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyStackManager : MonoBehaviour
{
    [SerializeField] private WorkbenchStackView moneyView;

    public void SpawnMoney(int amount)
    {
        // 1개만 생성
        GameObject obj = moneyView.AddItem();

        MoneyPickup pickup = obj.GetComponent<MoneyPickup>();
        if (pickup != null)
        {
            pickup.SetAmount(amount); // 금액만 설정
            pickup.SetOwner(moneyView);
        }
    }
}
