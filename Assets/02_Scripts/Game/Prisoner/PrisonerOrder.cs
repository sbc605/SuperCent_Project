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

    private MoneyStackManager moneyManager;
    [SerializeField] private GameObject moneyPrefab;
    [SerializeField] private int rewardMoney = 10;

    private PrisonerController prisoner;
    private Transform moneySpawnPoint;
    private PrisonerQueueManager manager;

    public int RequiredCount { get; private set; }
    public int CurrentCount { get; private set; }
    public bool IsCompleted { get; private set; }

    private void Start()
    {
        prisoner = GetComponent<PrisonerController>();
        moneyManager = FindFirstObjectByType<MoneyStackManager>();
    }

    public void Initialize(PrisonerQueueManager queueManager, Transform spawnPoint)
    {
        manager = queueManager;
        moneySpawnPoint = spawnPoint;
    }

    private void OnEnable()
    {
        ResetOrder();
    }

    public void ResetOrder()
    {
        RequiredCount = Random.Range(minNeed, maxNeed + 1);
        CurrentCount = 0;
        IsCompleted = false;

        Debug.Log($"[PrisonerOrder] Need: {RequiredCount}");
    }

    public bool TryDeliverOne()
    {
        if (IsCompleted)
            return false;

        CurrentCount++;

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

        if (moneyManager != null)
        {
            moneyManager.SpawnMoney(rewardMoney);
        }

        if (prisoner != null)
        {
            manager.OnOrderComplete(prisoner);
        }
    }
}
