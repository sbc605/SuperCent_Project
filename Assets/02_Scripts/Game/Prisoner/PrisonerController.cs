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
    private Animator anim;

    [SerializeField] private float moveSpeed = 2f;

    [Header("외형 변경")]
    [SerializeField] private GameObject normalVisual;
    [SerializeField] private GameObject handcuffedVisual;

    private PrisonerQueueManager queueManager;
    private Transform exitPoint;
    private Transform targetPoint;

    private bool isLeaving;
    private bool isMovingByCoroutine;

    private readonly int MoveHash = Animator.StringToHash("Move");


    void Start()
    {
        anim = GetComponent<Animator>();
        normalVisual.SetActive(true);
        handcuffedVisual.SetActive(false);
    }

    private void Update()
    {
        Move();
        UpdateAnimation();
    }

    public void Initialize(PrisonerQueueManager manager, Transform exit, Transform queuePoint)
    {
        queueManager = manager;
        exitPoint = exit;
        targetPoint = queuePoint;
        isLeaving = false;

        var order = GetComponent<PrisonerOrder>();
        if (order != null)
            order.Initialize(manager, manager.MoneySpawnPoint, manager.MoneyStackManager);
    }

    private void Move()
    {
        if (targetPoint == null)
            return;

        Vector3 beforePos = transform.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);

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
        isMovingByCoroutine = true;

        while (Vector3.Distance(transform.position, pos) > 0.05f)
        {
            Vector3 dir = pos - transform.position;
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

            transform.position = Vector3.MoveTowards(
                transform.position,
                pos,
                moveSpeed * Time.deltaTime
            );

            yield return null;
        }

        isMovingByCoroutine = false;
    }

    public void OnHandcuffReceived()
    {
        if (normalVisual != null)
            normalVisual.SetActive(false);

        if (handcuffedVisual != null)
            handcuffedVisual.SetActive(true);
    }

    private void UpdateAnimation()
    {
        if (anim == null)
            return;

        bool isMovingToTarget = false;

        if (targetPoint != null)
        {
            float distance = Vector3.Distance(transform.position, targetPoint.position);
            isMovingToTarget = distance > 0.05f;
        }

        bool isMoving = isMovingToTarget || isMovingByCoroutine;

        float targetMove = isMoving ? 1f : 0f;
        float currentMove = anim.GetFloat(MoveHash);
        float smoothMove = Mathf.Lerp(currentMove, targetMove, 10f * Time.deltaTime);

        anim.SetFloat(MoveHash, smoothMove);
    }
}

