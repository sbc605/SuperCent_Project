using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    [Header("Field Size")]
    [SerializeField] private Vector2 fieldSize = new Vector2(10f, 15f);

    [Header("Prefab")]
    [SerializeField] private GameObject[] rockPrefab;

    [Header("Auto Spawn")]
    [SerializeField] private bool spawnOnStart = true;
    [SerializeField] private float spacingX = 0.5f;
    [SerializeField] private float spacingZ = 0.5f;
    [SerializeField] private Vector2 startOffset = Vector2.zero;
    [SerializeField] private Transform rockRoot;

    private HashSet<Vector2Int> occupiedCells = new HashSet<Vector2Int>();

    private void Start()
    {
        if (spawnOnStart)
        {
            SpawnRocksWithSpacing();
        }
    }

    private void SpawnRocksWithSpacing()
    {
        int countX = Mathf.FloorToInt((fieldSize.x - startOffset.x) / spacingX);
        int countZ = Mathf.FloorToInt((fieldSize.y - startOffset.y) / spacingZ);

        for (int x = 0; x <= countX; x++)
        {
            for (int z = 0; z <= countZ; z++)
            {
                Vector2Int cell = new Vector2Int(x, z);

                if (!occupiedCells.Contains(cell))
                {
                    SpawnRock(cell);
                }
            }
        }
    }

    private void SpawnRock(Vector2Int cell)
    {
        Vector3 worldPos = CellToWorld(cell);
        int index = Random.Range(0, rockPrefab.Length);
        GameObject prefab = rockPrefab[index];

        Instantiate(prefab, worldPos, Quaternion.identity, rockRoot);
        occupiedCells.Add(cell);
    }

    private Vector3 CellToWorld(Vector2Int cell)
    {
        float originX = -fieldSize.x * 0.5f;
        float originZ = -fieldSize.y * 0.5f;

        float localX = originX + startOffset.x + cell.x * spacingX;
        float localZ = originZ + startOffset.y + cell.y * spacingZ;

        Vector3 localPos = new Vector3(localX, 0f, localZ);
        return transform.TransformPoint(localPos);
    }
}
