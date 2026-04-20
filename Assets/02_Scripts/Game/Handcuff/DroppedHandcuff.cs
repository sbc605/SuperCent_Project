using DG.Tweening;
using UnityEngine;

/// <summary>
/// 바닥(dropTargetPoint)에 놓인 수갑.
/// Receiver가 지정되면 수감자에게 이동하고 닿으면 주문 1 감소 후 사라짐.
/// </summary>
public class DroppedHandcuff : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;

    private Transform target;
    private PrisonerOrder targetOrder;
    private bool isMovingToPrisoner;

    public void SetTarget(Transform prisonerTarget, PrisonerOrder order)
    {
        target = prisonerTarget;
        targetOrder = order;
        isMovingToPrisoner = true;
    }

    // private void Update()
    // {
    //     if (!isMovingToPrisoner || target == null || targetOrder == null)
    //         return;

    //     transform.position = Vector3.MoveTowards(
    //         transform.position,
    //         target.position,
    //         moveSpeed * Time.deltaTime
    //     );

    //     Vector3 dir = target.position - transform.position;
    //     dir.y = 0f;
    //     if (dir.sqrMagnitude > 0.001f)
    //     {
    //         transform.rotation = Quaternion.Slerp(
    //             transform.rotation,
    //             Quaternion.LookRotation(dir),
    //             12f * Time.deltaTime
    //         );
    //     }

    //     if (Vector3.Distance(transform.position, target.position) < 0.08f)
    //     {
    //         bool delivered = targetOrder.TryDeliverOne();
    //         if (delivered)
    //         {
    //             Destroy(gameObject);
    //         }
    //     }
    // }

    public void JumpTo(Transform target, PrisonerOrder order)
    {
        Vector3 end = target.position;

        transform.DOJump(end, 1.2f, 1, 0.35f)
        .OnComplete(() =>
        {
            if (order != null && !order.IsCompleted)
            {
                order.TryDeliverOne();
            }

            Destroy(gameObject);
        });
    }
}