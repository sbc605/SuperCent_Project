using System.Collections;
using UnityEngine;

public class RockNode : MonoBehaviour
{
    [SerializeField] private float respawnTime = 5f;
    [SerializeField] private GameObject visualRoot;

    private bool canMine = true;

    public bool CanMine => canMine;

    public bool TryMine(PlayerInventory inventory)
    {
        if (!canMine)
            return false;

        if (!inventory.TryAddStone(1))
            return false;

        StartCoroutine(CoRespawn());
        canMine = false;

        if (visualRoot != null)
            visualRoot.SetActive(false);

        return true;
    }

    private IEnumerator CoRespawn()
    {
        yield return new WaitForSeconds(respawnTime);

        canMine = true;

        if (visualRoot != null)
            visualRoot.SetActive(true);
    }
}