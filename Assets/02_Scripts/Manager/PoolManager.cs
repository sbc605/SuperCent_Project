using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : GenericSingleton<PoolManager>
{
    public Dictionary<string, ObjectPool<GameObject>> pools = new();

    [Header("Pool Prefabs")]
    public GameObject[] rockPrefabs;

    [Header("Pool Root")]
    [SerializeField] private Transform poolRoot;

    [SerializeField] private int defaultCapacity = 20;
    [SerializeField] private int maxPoolSize = 100;

    protected override void Awake()
    {
        base.Awake();

        foreach (GameObject prefab in rockPrefabs)
        {
            ObjectPool<GameObject> newPool = new ObjectPool<GameObject>(() => CreateObject(prefab), GetObject, ReleaseObject, maxSize: maxPoolSize);

            pools.Add(prefab.name, newPool);
        }
    }

    private GameObject CreateObject(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab, poolRoot);
        obj.SetActive(false);
        return obj;
    }

    private void GetObject(GameObject obj)
    {
        obj.SetActive(true);
    }

    private void ReleaseObject(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(poolRoot);
    }

    private void OnDestroyObject(GameObject obj)
    {
        Destroy(obj);
    }


    public GameObject GetPoolObject(string prefabName, Vector3 position, Quaternion rotation)
    {
        if (!pools.TryGetValue(prefabName, out var pool))
            return null;

        GameObject obj = pool.Get();
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        return obj;
    }

    public void ReleasePoolObject(string prefabName, GameObject obj)
    {
        if (obj == null) return;

        if (!pools.TryGetValue(prefabName, out var pool))
        {
            Destroy(obj);
            return;
        }

        pool.Release(obj);
    }
}
