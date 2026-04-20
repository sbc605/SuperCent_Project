using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyStackView : MonoBehaviour
{
    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private Transform stackRoot;
    [SerializeField] private GameObject moneyVisualPrefab;
    [SerializeField] private float yOffset = 0.2f;

    private readonly List<GameObject> spawnedItems = new();

    private void OnEnable()
    {
        if (inventory != null)
            inventory.OnMoneyStackChanged += RefreshView;
    }

    private void OnDisable()
    {
        if (inventory != null)
            inventory.OnMoneyStackChanged -= RefreshView;
    }

    private void Start()
    {
        if (inventory == null)
            return;

        RefreshView(inventory.CurrentMoneyStackCount, inventory.MaxMoneyCount);
    }

    private void RefreshView(int current, int max)
    {
        Debug.Log($"[MoneyStackView] RefreshView: {current}/{max}");

        spawnedItems.RemoveAll(x => x == null);

        while (spawnedItems.Count < current)
        {
            GameObject obj = Instantiate(moneyVisualPrefab, stackRoot);
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
            spawnedItems[i].transform.localPosition = new Vector3(0f, i * yOffset, 0f);
            spawnedItems[i].transform.localRotation = Quaternion.identity;
        }
    }
}
