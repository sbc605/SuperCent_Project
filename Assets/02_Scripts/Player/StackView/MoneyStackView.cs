using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyStackView : StackViewBase
{
    protected override int CurrentCount
    {
        get
        {
            if (inventory == null)
                return 0;

            return inventory.CurrentMoneyStackCount;
        }
    }

    protected override int MaxCount
    {
        get
        {
            if (inventory == null)
                return 0;

            return inventory.MaxMoneyCount;
        }
    }

    protected override void Subscribe()
    {
        if (inventory != null)
            inventory.OnMoneyStackChanged += RefreshView;
    }

    protected override void Unsubscribe()
    {
        if (inventory != null)
            inventory.OnMoneyStackChanged -= RefreshView;
    }
}
