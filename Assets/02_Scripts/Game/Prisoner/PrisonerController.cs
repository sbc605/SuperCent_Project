using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 수감자 이동 담당
/// Queue Index, Exit 이동, targetPoint 제어
/// </summary>
public class PrisonerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;

    private PrisonerQueueManager queueManager;
    private Transform exitPoint;
    private Transform targetPoint;

    private bool isLeaving;

    private void Update()
    {
        if (targetPoint == null)
            return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPoint.position,
            moveSpeed * Time.deltaTime
        );

        Vector3 dir = targetPoint.position - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                10f * Time.deltaTime
            );
        }

        // exit 도착 후 queueManager 호출
        if (isLeaving && Vector3.Distance(transform.position, targetPoint.position) < 0.05f)
        {
            isLeaving = false;
            queueManager.ReleasePrisoner(this);
        }
    }

    public void Initialize(PrisonerQueueManager manager, Transform exit, Transform queuePoint)
    {
        queueManager = manager;
        exitPoint = exit;
        targetPoint = queuePoint;
        isLeaving = false;

        var order = GetComponent<PrisonerOrder>();
        if (order != null)
            order.Initialize(manager, manager.MoneySpawnPoint);
    }

    public void SetQueuePoint(Transform point)
    {
        targetPoint = point;
    }

    /// <summary>
    /// exitPoint로 먼저 이동
    /// </summary>
    public void LeaveQueue()
    {
        isLeaving = true;
        targetPoint = exitPoint;
    }

    public void MoveTo(Vector3 pos)
    {
        targetPoint = null;
        StartCoroutine(CoMove(pos));
    }

    private IEnumerator CoMove(Vector3 pos)
    {
        while (Vector3.Distance(transform.position, pos) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                pos,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }
    }
}

