using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// 수감자 위치에서 돈 생성 → 점프,이동 연출 → moneyPos에 넣기
/// </summary>
public class MoneyStackManager : MonoBehaviour
{
    [SerializeField] private WorkbenchStackView moneyView;
    [SerializeField] private GameObject moneyPrefab;

    [Header("Animation")]
    [SerializeField] private float jumpPower = 1.2f;
    [SerializeField] private float moveDuration = 0.45f;

    public void SpawnMoney(int amount, Transform from)
    {
        if (moneyView == null || moneyPrefab == null || from == null)
            return;

        GameObject obj = Instantiate(moneyPrefab, from.position, Quaternion.identity);

        MoneyPickup pickup = obj.GetComponent<MoneyPickup>();
        if (pickup != null)
        {
            pickup.SetAmount(amount); // 금액 설정
            pickup.SetOwner(moneyView);
            pickup.SetPickupEnabled(false);
        }

        Vector3 targetPos = moneyView.Root.position;

        obj.transform.DOJump(targetPos, jumpPower, 1, moveDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                moneyView.AddExistingItem(obj);

                if (pickup != null)
                    pickup.SetPickupEnabled(true);
            });
    }
}
