using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 수감자 줄 관리 전용
/// 등록, 재배치, 맨 앞 주문 접근, 퇴장 처리 담당
/// Waiting / Order / Exit
/// </summary>
public class PrisonerQueueManager : MonoBehaviour
{
    [Header("Points")]
    [SerializeField] private Transform waitingPoint;
    [SerializeField] private Transform orderPoint;
    [SerializeField] private Transform exitPoint;

    [Header("Money")]
    [SerializeField] private Transform moneySpawnPoint;
    [SerializeField] private MoneyStackManager moneyStackManager;

    [SerializeField] private PrisonerSpawner spawner;

    private PrisonerController waitingPrisoner;
    private PrisonerController currentPrisoner;

    public Transform MoneySpawnPoint => moneySpawnPoint;
    public MoneyStackManager MoneyStackManager => moneyStackManager;

    private void Start()
    {
        SpawnInitial();
    }

    private void SpawnInitial()
    {
        currentPrisoner = spawner.SpawnPrisoner();
        if (currentPrisoner != null)
            currentPrisoner.Initialize(this, exitPoint, orderPoint);

        waitingPrisoner = spawner.SpawnPrisoner();
        if (waitingPrisoner != null)
            waitingPrisoner.Initialize(this, exitPoint, waitingPoint);
    }

    /// <summary>
    /// 현재 주문자 반환
    /// </summary>
    public PrisonerOrder GetFrontOrder()
    {
        if (currentPrisoner == null)
            return null;

        return currentPrisoner.GetComponent<PrisonerOrder>();
    }

    /// <summary>
    /// 주문 완료 시 호출
    /// </summary>
    public void OnOrderComplete(PrisonerController prisoner)
    {
        if (prisoner != currentPrisoner)
            return;

        // 1. 현재 Order → Exit
        currentPrisoner.LeaveQueue();

        // 2. waiting이 없으면 먼저 생성
        if (waitingPrisoner == null)
        {
            waitingPrisoner = spawner.SpawnPrisoner();
            if (waitingPrisoner != null)
                waitingPrisoner.Initialize(this, exitPoint, waitingPoint);
        }

        // 3. waiting → current
        if (waitingPrisoner != null)
        {
            currentPrisoner = waitingPrisoner;
            currentPrisoner.SetQueuePoint(orderPoint);
        }
        else
        {
            Debug.LogError("[QueueManager] waitingPrisoner is null!");
            return;
        }

        // 4. 새 waiting 생성
        waitingPrisoner = spawner.SpawnPrisoner();
        if (waitingPrisoner != null)
            waitingPrisoner.Initialize(this, exitPoint, waitingPoint);
    }

    /// <summary>
    /// exit 도착 시 호출
    /// </summary>
    public void ReleasePrisoner(PrisonerController prisoner)
    {
        MoveToExitArea(prisoner);
    }

    private void MoveToExitArea(PrisonerController prisoner)
    {
        Vector3 offset = new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f));

        prisoner.MoveTo(exitPoint.position + offset);
    }
}