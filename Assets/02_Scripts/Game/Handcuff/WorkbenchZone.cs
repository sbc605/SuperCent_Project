using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 작업대 전체 흐름 제어: 돌이 들어오면 수갑으로 교체
/// </summary>
public class WorkbenchZone : MonoBehaviour
{
    [Header("Drop Settings")]
    [SerializeField] private float dropInterval = 0.15f;

    [Header("Craft Settings")]
    [SerializeField] private float craftInterval = 0.3f;

    [Header("Refs")]
    [SerializeField] private WorkbenchStackView stoneView;
    [SerializeField] private WorkbenchStackView handcuffView;

    private PlayerInventory currentPlayer;
    private Coroutine dropRoutine;
    private Coroutine craftRoutine;

    private void Start()
    {
        if (craftRoutine == null)
            craftRoutine = StartCoroutine(CoCraftHandcuff());
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory inventory = other.GetComponentInParent<PlayerInventory>();
        if (inventory == null)
            return;

        currentPlayer = inventory;

        if (dropRoutine == null)
            dropRoutine = StartCoroutine(CoDropStone());
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerInventory inventory = other.GetComponentInParent<PlayerInventory>();
        if (inventory == null || inventory != currentPlayer)
            return;

        currentPlayer = null;

        if (dropRoutine != null)
        {
            StopCoroutine(dropRoutine);
            dropRoutine = null;
        }
    }

    private IEnumerator CoDropStone()
    {
        while (currentPlayer != null)
        {
            if (currentPlayer.TryRemoveStone(1))
            {
                stoneView.AddItem();
            }

            yield return new WaitForSeconds(dropInterval);
        }

        dropRoutine = null;
    }

    private IEnumerator CoCraftHandcuff()
    {
        while (true)
        {
            if (stoneView.Count > 0)
            {
                bool removed = stoneView.RemoveFirst();

                if (removed)
                {
                    GameObject obj = handcuffView.AddItem();
                    HandcuffNode pickup = obj.GetComponent<HandcuffNode>();

                    if (pickup != null)
                        pickup.SetOwner(handcuffView);
                }
            }

            yield return new WaitForSeconds(craftInterval);
        }
    }
}
