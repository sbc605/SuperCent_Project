using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 손에 수갑이 쌓이는 걸 보여주는 역할
/// 플레이어 프리팹에 붙여 사용
/// </summary>
public class HandcuffStackView : StackViewBase
{
    protected override int CurrentCount
    {
        get
        {
            if (inventory == null)
                return 0;

            return inventory.CurrentHandcuffCount;
        }
    }

    protected override int MaxCount
    {
        get
        {
            if (inventory == null)
                return 0;

            return inventory.MaxHandcuffCount;
        }
    }

    protected override void Subscribe()
    {
        if (inventory != null)
            inventory.OnHandcuffChanged += RefreshView;
    }

    protected override void Unsubscribe()
    {
        if (inventory != null)
            inventory.OnHandcuffChanged -= RefreshView;
    }
}