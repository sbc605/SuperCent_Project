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

    [Header("Drop Visual")]
    [SerializeField] private Transform dropSpawnPoint;     // 손 위치
    [SerializeField] private Transform dropTargetPoint;    // 바닥 위치
    [SerializeField] private GameObject handcuffVisualPrefab;
    [SerializeField] private WorkbenchStackView dropStackView;

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
                Sfx.Play("item_clip");
                // 연출용 오브젝트 생성
                GameObject obj = Instantiate(handcuffVisualPrefab, dropSpawnPoint.position, Quaternion.identity);

                ItemDropAnimator anim = obj.AddComponent<ItemDropAnimator>();

                // 떨어진 후 스택에 추가
                anim.DropTo(dropTargetPoint, () =>
    {
        // 바닥에 도착한 오브젝트를 DroppedHandcuff로 사용
        DroppedHandcuff dropped = obj.GetComponent<DroppedHandcuff>();

        // 현재 주문자 가져오기
        var targetOrder = queueManager.GetFrontOrder();

        if (targetOrder != null)
        {
            Transform targetTf = targetOrder.transform;

            dropped.JumpTo(targetTf, targetOrder);
        }
    });
            }

            yield return new WaitForSeconds(deliverInterval);
        }

        deliverRoutine = null;
    }
}
