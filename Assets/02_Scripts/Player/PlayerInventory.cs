using System;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private int maxStoneCount = 10;

    public int CurrentStoneCount { get; private set; }
    public int MaxStoneCount => maxStoneCount;

    public event Action<int, int> OnStoneChanged;
    public event Action OnStoneMax;

    public bool TryAddStone(int amount = 1)
    {
        if (CurrentStoneCount >= maxStoneCount)
        {
            OnStoneMax?.Invoke();
            return false;
        }

        CurrentStoneCount = Mathf.Min(CurrentStoneCount + amount, maxStoneCount);

        Debug.Log($"[PlayerInventory] Stone Added: {CurrentStoneCount}/{maxStoneCount}");
        OnStoneChanged?.Invoke(CurrentStoneCount, maxStoneCount);

        return true;
    }

    public bool TryRemoveStone(int amount = 1)
    {
        if (CurrentStoneCount < amount)
            return false;

        CurrentStoneCount -= amount;
        OnStoneChanged?.Invoke(CurrentStoneCount, maxStoneCount);
        return true;
    }

    public void SetMaxStoneCount(int value)
    {
        maxStoneCount = value;
        CurrentStoneCount = Mathf.Min(CurrentStoneCount, maxStoneCount);
        OnStoneChanged?.Invoke(CurrentStoneCount, maxStoneCount);
    }
}
