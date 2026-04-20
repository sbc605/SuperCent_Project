using UnityEngine;

/// <summary>
/// dropTargetPoint에 쌓인 수갑을 현재 주문 중인 수감자에게 보내는 역할
/// </summary>
public class PrisonerHandcuffReceiver : MonoBehaviour
{
    [SerializeField] private PrisonerQueueManager queueManager;
    [SerializeField] private Transform prisonerReceivePoint;

    public void SendHandcuff(GameObject droppedHandcuff)
    {
        if (droppedHandcuff == null)
            return;

        PrisonerOrder order = queueManager.GetFrontOrder();
        if (order == null || order.IsCompleted)
            return;

        DroppedHandcuff dropped = droppedHandcuff.GetComponent<DroppedHandcuff>();
        if (dropped == null)
            return;

        dropped.SetTarget(prisonerReceivePoint, order);
    }
}