using System;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Capacity")]
    [SerializeField] private int maxStoneCount = 10;
    [SerializeField] private int maxHandcuffCount = 10;
    [SerializeField] private int maxMoneyCount = 10;

    public int CurrentStoneCount { get; private set; }
    public int CurrentHandcuffCount { get; private set; }
    public int CurrentMoneyStackCount { get; private set; } // 돈 프리팹 개수
    public int TotalMoney { get; private set; } // 실제 총액

    public int MaxStoneCount => maxStoneCount;
    public int MaxHandcuffCount => maxHandcuffCount;
    public int MaxMoneyCount => maxMoneyCount;

    public event Action<int, int> OnStoneChanged;
    public event Action<int, int> OnHandcuffChanged;
    public event Action<int, int> OnMoneyStackChanged;
    public event Action<int> OnMoneyValueChanged;

    public event Action OnStoneMax;
    public event Action OnHandcuffMax;
    public event Action OnMoneyMax;

    #region 바위
    public bool TryAddStone(int amount = 1)
    {
        if (CurrentStoneCount >= maxStoneCount)
        {
            OnStoneMax?.Invoke();
            return false;
        }

        CurrentStoneCount = Mathf.Min(CurrentStoneCount + amount, maxStoneCount);
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
    #endregion

    #region 수갑
    public bool TryAddHandcuff(int amount = 1)
    {
        if (CurrentHandcuffCount >= maxHandcuffCount)
        {
            OnHandcuffMax?.Invoke();
            return false;
        }

        CurrentHandcuffCount = Mathf.Min(CurrentHandcuffCount + amount, maxHandcuffCount);
        OnHandcuffChanged?.Invoke(CurrentHandcuffCount, maxHandcuffCount);
        return true;
    }

    public bool TryRemoveHandcuff(int amount = 1)
    {
        if (CurrentHandcuffCount < amount)
            return false;

        CurrentHandcuffCount -= amount;
        OnHandcuffChanged?.Invoke(CurrentHandcuffCount, maxHandcuffCount);
        return true;
    }

    public void SetMaxHandcuffCount(int value)
    {
        maxHandcuffCount = value;
        CurrentHandcuffCount = Mathf.Min(CurrentHandcuffCount, maxHandcuffCount);
        OnHandcuffChanged?.Invoke(CurrentHandcuffCount, maxHandcuffCount);
    }
    #endregion

    #region 돈
    public bool TryAddMoney(int stackAmount = 1)
    {
        if (CurrentMoneyStackCount >= maxMoneyCount)
        {
            OnMoneyMax?.Invoke();
            return false;
        }

        CurrentMoneyStackCount = Mathf.Min(CurrentMoneyStackCount + stackAmount, maxMoneyCount);
        OnMoneyStackChanged?.Invoke(CurrentMoneyStackCount, maxMoneyCount);
        return true;
    }

    public bool TryRemoveMoneyStack(int amount = 1)
    {
        if (CurrentMoneyStackCount < amount)
            return false;

        CurrentMoneyStackCount -= amount;
        OnMoneyStackChanged?.Invoke(CurrentMoneyStackCount, maxMoneyCount);
        return true;
    }

    public void AddMoneyValue(int value)
    {
        TotalMoney += value;
        OnMoneyValueChanged?.Invoke(TotalMoney);
    }
    #endregion
}
