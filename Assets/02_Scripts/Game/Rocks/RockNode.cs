using System.Collections;
using UnityEngine;

/// <summary>
/// 바위 프리팹에 붙여서 사용
/// </summary>
public class RockNode : MonoBehaviour
{
    [SerializeField] private float respawnTime = 10f;
    [SerializeField] private GameObject visualRoot;
    [SerializeField] private Collider rockCollider;

    private bool canMine = true;

    public bool CanMine => canMine;

    private void Reset()
    {
        rockCollider = GetComponent<Collider>();
        visualRoot = gameObject;
    }


    public bool TryMine(PlayerInventory inventory)
    {
        if (!canMine) return false;

        if (inventory == null)
        {
            Debug.LogError("[RockNode] inventory is null");
            return false;
        }

        if (!inventory.TryAddStone(1))
            return false;

        canMine = false;

        if (visualRoot != null)
            visualRoot.SetActive(false);

        if (rockCollider != null)
            rockCollider.enabled = false;

        Debug.Log($"[RockNode] Mined: {name}");

        StartCoroutine(CoRespawn());
        return true;
    }

    private IEnumerator CoRespawn()
    {
        yield return new WaitForSeconds(respawnTime);

        canMine = true;

        if (visualRoot != null)
            visualRoot.SetActive(true);

        if (rockCollider != null)
            rockCollider.enabled = true;

        Debug.Log($"[RockNode] Respawn: {name}");
    }
}