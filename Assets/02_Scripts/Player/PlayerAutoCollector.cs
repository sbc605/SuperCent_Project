using System.Collections.Generic;
using UnityEngine;

public class PlayerAutoCollector : MonoBehaviour
{
    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private float mineInterval = 0.4f;

    private float mineTimer;
    private readonly List<RockNode> nearbyStones = new();

    private void Update()
    {
        mineTimer -= Time.deltaTime;

        if (mineTimer > 0f)
            return;

        RockNode target = GetClosestMineableStone();
        if (target == null)
            return;

        bool success = target.TryMine(inventory);

        if (success)
        {
            mineTimer = mineInterval;
        }
        else
        {
            // 인벤토리가 가득 찼다면 여기서 MAX UI 띄우기
            mineTimer = 0.2f;
        }
    }

    private RockNode GetClosestMineableStone()
    {
        nearbyStones.RemoveAll(x => x == null || !x.CanMine);

        RockNode closest = null;
        float minDist = float.MaxValue;

        foreach (var stone in nearbyStones)
        {
            float dist = (stone.transform.position - transform.position).sqrMagnitude;
            if (dist < minDist)
            {
                minDist = dist;
                closest = stone;
            }
        }

        return closest;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out RockNode stone))
        {
            if (!nearbyStones.Contains(stone))
                nearbyStones.Add(stone);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out RockNode stone))
        {
            nearbyStones.Remove(stone);
        }
    }
}