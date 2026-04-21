using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StackViewBase : MonoBehaviour
{
    [Header("Common")]
    [SerializeField] protected PlayerInventory inventory;
    [SerializeField] protected Transform stackRoot;
    [SerializeField] protected GameObject visualPrefab;
    [SerializeField] protected float yOffset = 0.2f;

    [Header("Stack Option")]
    [SerializeField] protected bool stackFromTop = false;

    protected readonly List<GameObject> spawnedItems = new();

    protected abstract int CurrentCount { get; }
    protected abstract int MaxCount { get; }

    protected abstract void Subscribe();
    protected abstract void Unsubscribe();

    protected virtual void OnEnable()
    {
        Subscribe();
    }

    protected virtual void OnDisable()
    {
        Unsubscribe();
    }

    protected virtual void Start()
    {
        RefreshView(CurrentCount, MaxCount);
    }

    protected void RefreshView(int current, int max)
    {
        if (stackRoot == null || visualPrefab == null)
            return;

        spawnedItems.RemoveAll(x => x == null);

        while (spawnedItems.Count < current)
        {
            GameObject obj = Instantiate(visualPrefab, stackRoot);
            spawnedItems.Add(obj);
        }

        while (spawnedItems.Count > current)
        {
            GameObject last = spawnedItems[^1];
            spawnedItems.RemoveAt(spawnedItems.Count - 1);
            Destroy(last);
        }

        UpdatePositions(current);
    }

    protected virtual void UpdatePositions(int current)
    {
        for (int i = 0; i < spawnedItems.Count; i++)
        {
            if (spawnedItems[i] == null)
                continue;

            spawnedItems[i].transform.localPosition = GetStackPosition(i, current);
            spawnedItems[i].transform.localRotation = Quaternion.identity;
        }
    }

    protected virtual Vector3 GetStackPosition(int index, int current)
    {
        if (stackFromTop)
        {
            float baseY = (current - 1) * yOffset;
            return new Vector3(0f, baseY - index * yOffset, 0f);
        }

        return new Vector3(0f, index * yOffset, 0f);
    }
}
