using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 손에 수갑이 쌓이는 걸 보여주는 역할
/// 플레이어 프리팹에 붙여 사용
/// </summary>
public class HandcuffStackView : MonoBehaviour
{
    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private Transform stackRoot;
    [SerializeField] private GameObject handcuffVisualPrefab;
    [SerializeField] private float yOffset = 0.2f;

    private readonly List<GameObject> spawnedItems = new();

    private void OnEnable()
    {
        if (inventory != null)
            inventory.OnHandcuffChanged += RefreshView;
    }

    private void OnDisable()
    {
        if (inventory != null)
            inventory.OnHandcuffChanged -= RefreshView;
    }

    private void Start()
    {
        if (inventory == null)
            return;

        RefreshView(inventory.CurrentHandcuffCount, inventory.MaxHandcuffCount);
    }

    private void RefreshView(int current, int max)
    {
        Debug.Log($"[HandcuffStackView] RefreshView: {current}/{max}");

        spawnedItems.RemoveAll(x => x == null);

        while (spawnedItems.Count < current)
        {
            GameObject obj = Instantiate(handcuffVisualPrefab, stackRoot);
            spawnedItems.Add(obj);
        }

        while (spawnedItems.Count > current)
        {
            GameObject last = spawnedItems[^1];
            spawnedItems.RemoveAt(spawnedItems.Count - 1);
            Destroy(last);
        }

        for (int i = 0; i < spawnedItems.Count; i++)
        {
            if (spawnedItems[i] == null) continue;
            float baseY = (current - 1) * yOffset;
            spawnedItems[i].transform.localPosition = new Vector3(0f, baseY - i * yOffset, 0f);
            spawnedItems[i].transform.localRotation = Quaternion.identity;
        }
    }
}