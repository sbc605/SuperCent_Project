using System.Collections.Generic;
using UnityEngine;

public class RockStackView : MonoBehaviour
{
    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private Transform stackRoot;
    [SerializeField] private GameObject stoneVisualPrefab;
    [SerializeField] private float yOffset = 0.25f;

    private readonly List<GameObject> spawnedStones = new();

    private void OnEnable()
    {
        if (inventory != null)
            inventory.OnStoneChanged += RefreshView;
    }

    private void OnDisable()
    {
        if (inventory != null)
            inventory.OnStoneChanged -= RefreshView;
    }

    private void Start()
    {
        if (inventory == null)
        {
            Debug.LogError("[RockStackView] inventory is null");
            return;
        }

        RefreshView(inventory.CurrentStoneCount, inventory.MaxStoneCount);
    }

    private void RefreshView(int current, int max)
    {
        Debug.Log($"[RockStackView] RefreshView: {current}/{max}");

        while (spawnedStones.Count < current)
        {
            GameObject obj = Instantiate(stoneVisualPrefab, stackRoot);
            spawnedStones.Add(obj);
        }

        while (spawnedStones.Count > current)
        {
            GameObject last = spawnedStones[^1];
            spawnedStones.RemoveAt(spawnedStones.Count - 1);
            Destroy(last);
        }

        for (int i = 0; i < spawnedStones.Count; i++)
        {
            spawnedStones[i].transform.localPosition = new Vector3(0f, i * yOffset, 0f);
            spawnedStones[i].transform.localRotation = Quaternion.identity;
        }
    }
}