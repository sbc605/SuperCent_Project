using System.Collections.Generic;
using UnityEngine;

public class PlayerAutoCollector : MonoBehaviour
{
    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private PlayerController playerController;

    private readonly List<RockNode> nearbyStones = new();

    private void Update()
    {
        RockNode target = GetClosestMineableStone();
        if (target == null) return;

        bool success = target.TryMine(inventory);

        if (success)
        {
            nearbyStones.Remove(target); // 자신이 닿은 바위 채굴하면 리스트에서 직접 제거
            if (playerController != null)
                playerController.PlayMineAction();
            Debug.Log($"[PlayerAutoCollector] Mine Success: {target.name}");
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
        RockNode stone = other.GetComponentInParent<RockNode>();

        if (stone != null && !nearbyStones.Contains(stone))
        {
            nearbyStones.Add(stone);
            Debug.Log($"[PlayerAutoCollector] Enter: {stone.name}");
        }
    }

    /// <summary>
    /// 리스폰 후 플레이어가 자리에 가만히 있어도 다시 리스트에 추가됨
    /// </summary>
    private void OnTriggerStay(Collider other)
    {
        RockNode stone = other.GetComponentInParent<RockNode>();

        if (stone != null && stone.CanMine && !nearbyStones.Contains(stone))
        {
            nearbyStones.Add(stone);
            Debug.Log($"[PlayerAutoCollector] Stay Add: {stone.name}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        RockNode stone = other.GetComponentInParent<RockNode>();

        if (stone != null)
        {
            nearbyStones.Remove(stone);
            Debug.Log($"[PlayerAutoCollector] Exit: {stone.name}");
        }
    }
}