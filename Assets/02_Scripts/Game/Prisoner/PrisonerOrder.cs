using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 필요한 수갑 수 랜덤 생성
/// 완료 시 돈 생성
/// </summary>
public class PrisonerOrder : MonoBehaviour
{
    [SerializeField] private int minNeed = 1;
    [SerializeField] private int maxNeed = 5;

    [SerializeField] private int rewardMoney = 10;

    [Header("UI")]
    [SerializeField] private PrisonerOrderUI orderUI;

    private Transform moneySpawnPoint;
    private MoneyStackManager moneyStackManager;

    private PrisonerController prisoner;
    private PrisonerQueueManager manager;

    public int RequiredCount { get; private set; }
    public int CurrentCount { get; private set; }
    public bool IsCompleted { get; private set; }

    public int RemainCount => Mathf.Max(RequiredCount - CurrentCount, 0);

    private void Awake()
    {
        prisoner = GetComponent<PrisonerController>();
    }

    public void Initialize(PrisonerQueueManager queueManager, Transform spawnPoint, MoneyStackManager stackManager)
    {
        manager = queueManager;
        moneySpawnPoint = spawnPoint;
        moneyStackManager = stackManager;
        ResetOrder();
    }

    public void ResetOrder()
    {
        RequiredCount = Random.Range(minNeed, maxNeed + 1);
        CurrentCount = 0;
        IsCompleted = false;

        RefreshUI();
        Debug.Log($"[PrisonerOrder] Need: {RequiredCount}");
    }

    public bool TryDeliverOne()
    {
        if (IsCompleted)
            return false;

        CurrentCount++;
        RefreshUI();

        Debug.Log($"[PrisonerOrder] Delivered: {CurrentCount}/{RequiredCount}");

        if (CurrentCount >= RequiredCount)
        {
            CompleteOrder();
        }

        return true;
    }

    private void CompleteOrder()
    {
        IsCompleted = true;
        RefreshUI();

        // 외형 변경
        if (prisoner != null)
            prisoner.OnHandcuffReceived();

        if (moneyStackManager != null && moneySpawnPoint != null)
        {
            moneyStackManager.SpawnMoney(rewardMoney, moneySpawnPoint);
        }

        if (prisoner != null && manager != null)
        {
            manager.OnOrderComplete(prisoner);
        }
    }

    private void RefreshUI()
    {
        if (orderUI == null)
            return;

        if (IsCompleted)
        {
            orderUI.Hide();
            return;
        }

        orderUI.Show(RemainCount, RequiredCount);
    }
}
