using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 작업대 - 돌,수갑 관리
/// 돌은 전체 개수에 따라 2줄로 배치합니다.
/// y축 적층 기능
/// </summary>
public class WorkbenchStackView : MonoBehaviour
{
    public enum LayoutType
    {
        TwoPerLayer,     // 돌, 돈
        VerticalSingle   // 수갑
    }

    [SerializeField] private Transform root;
    [SerializeField] private GameObject prefab;

    [Header("Layout")]
    [SerializeField] private LayoutType layoutType = LayoutType.TwoPerLayer;
    [SerializeField] private float xSpacing = 0.35f;
    [SerializeField] private float yOffset = 0.18f;

    private readonly List<GameObject> items = new();

    public int Count => items.Count;

    public GameObject AddItem()
    {
        GameObject obj = Instantiate(prefab, root);
        items.Add(obj);
        RefreshLayout();
        return obj;
    }

    public bool RemoveFirst()
    {
        if (items.Count == 0)
            return false;

        GameObject first = items[0];
        items.RemoveAt(0);
        Destroy(first);
        RefreshLayout();
        return true;
    }

    public bool RemoveLast()
    {
        if (items.Count == 0)
            return false;

        GameObject last = items[^1];
        items.RemoveAt(items.Count - 1);
        Destroy(last);
        RefreshLayout();
        return true;
    }

    public void RemoveItem(GameObject target)
    {
        if (target == null)
            return;

        if (items.Remove(target))
        {
            Destroy(target);
            RefreshLayout();
        }
    }

    private void RefreshLayout()
    {
        items.RemoveAll(x => x == null);

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null) continue;

            switch (layoutType)
            {
                case LayoutType.TwoPerLayer:
                    int layer = i / 2;
                    int indexInLayer = i % 2;

                    float y = layer * yOffset;
                    float z = (indexInLayer == 0) ? -xSpacing * 0.5f : xSpacing * 0.5f;

                    items[i].transform.localPosition = new Vector3(0f, y, z);
                    break;

                case LayoutType.VerticalSingle:
                    float y2 = i * yOffset;
                    items[i].transform.localPosition = new Vector3(0f, y2, 0f);
                    break;
            }

            items[i].transform.localRotation = Quaternion.identity;
        }
    }
}
