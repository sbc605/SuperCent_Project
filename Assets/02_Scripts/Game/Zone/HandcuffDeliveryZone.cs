using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 수갑 전달하는 곳
/// </summary>
public class HandcuffDeliveryZone : MonoBehaviour
{
    [SerializeField] private PrisonerQueueManager queueManager;
    [SerializeField] private float deliverInterval = 0.2f;

    private PlayerInventory currentPlayer;
    private Coroutine deliverRoutine;

    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory inventory = other.GetComponentInParent<PlayerInventory>();
        if (inventory == null)
            return;

        currentPlayer = inventory;

        if (deliverRoutine == null)
            deliverRoutine = StartCoroutine(CoDeliver());
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerInventory inventory = other.GetComponentInParent<PlayerInventory>();
        if (inventory == null || inventory != currentPlayer)
            return;

        currentPlayer = null;

        if (deliverRoutine != null)
        {
            StopCoroutine(deliverRoutine);
            deliverRoutine = null;
        }
    }

    private IEnumerator CoDeliver()
    {
        while (currentPlayer != null)
        {
            PrisonerOrder order = queueManager.GetFrontOrder();

            if (order == null || order.IsCompleted)
            {
                yield return null;
                continue;
            }

            // 수량이 없으면 멈춤
            if (currentPlayer.CurrentHandcuffCount <= 0)
            {
                yield return null;
                continue;
            }

            bool removed = currentPlayer.TryRemoveHandcuff(1);
            if (removed)
            {
                bool delivered = order.TryDeliverOne();

                if (!delivered)
                {
                    // 전달 실패면 롤백
                    currentPlayer.TryAddHandcuff(1);
                }
            }

            yield return new WaitForSeconds(deliverInterval);
        }

        deliverRoutine = null;
    }
}
