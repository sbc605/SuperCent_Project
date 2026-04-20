using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 수감자 생성 담당
/// 생성된 수감자를 QueueManager에 등록한다.
/// </summary>
public class PrisonerSpawner : MonoBehaviour
{
    [SerializeField] private PrisonerController prisonerPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private int startCount = 1;
    [SerializeField] private int maxCount = 10;

    [SerializeField] private PrisonerQueueManager queueManager;

    private readonly List<PrisonerController> spawnedPrisoners = new();

    public IReadOnlyList<PrisonerController> SpawnedPrisoners => spawnedPrisoners;

    private void Start()
    {
        for (int i = 0; i < startCount; i++)
        {
            SpawnPrisoner();
        }
    }

    public PrisonerController SpawnPrisoner()
    {
        if (prisonerPrefab == null)
        {
            Debug.LogError("[PrisonerSpawner] prisonerPrefab is null");
            return null;
        }

        if (spawnPoint == null)
        {
            Debug.LogError("[PrisonerSpawner] spawnPoint is null");
            return null;
        }

        if (queueManager == null)
        {
            Debug.LogError("[PrisonerSpawner] queueManager is null");
            return null;
        }

        if (spawnedPrisoners.Count >= maxCount)
        {
            Debug.LogWarning("[Spawner] Max count reached!");
            return null;
        }

        PrisonerController prisoner = Instantiate(prisonerPrefab, spawnPoint.position, spawnPoint.rotation);

        spawnedPrisoners.Add(prisoner);

        return prisoner;
    }

    public void UnregisterPrisoner(PrisonerController prisoner)
    {
        if (prisoner == null)
            return;

        spawnedPrisoners.Remove(prisoner);
    }

    public bool CanSpawnMore()
    {
        return spawnedPrisoners.Count < maxCount;
    }
}