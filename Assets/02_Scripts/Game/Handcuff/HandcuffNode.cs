using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 수갑 프리팹에 붙여 사용
/// 플레이어가 닿으면 수갑이 인벤토리로 들어간다.
/// </summary>
public class HandcuffNode : MonoBehaviour
{
    private WorkbenchStackView owner;
    private bool isPicked = false;

    /// <summary>
    /// 수갑이 List로 관리되므로 Destroy 때 재정렬 필요
    /// </summary>
    public void SetOwner(WorkbenchStackView stackView)
    {
        owner = stackView;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isPicked) return;

        PlayerInventory inventory = other.GetComponentInParent<PlayerInventory>();
        if (inventory == null)
            return;

        if (inventory.TryAddHandcuff(1))
        {
            isPicked = true;
            Sfx.Play("item_clip");
            
            if (owner != null)
                owner.RemoveItem(gameObject);
            else
                Destroy(gameObject);
        }
    }
}
