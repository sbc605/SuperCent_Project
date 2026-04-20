using UnityEngine;

/// <summary>
/// 플레이어 앞에서 목표 방향 안내 화살표
/// </summary>
public class PlayerDirectionGuideArrow : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform target;

    [Header("Position")]
    [SerializeField] private float forwardDistance = 1.5f;
    [SerializeField] private float height = 0.6f;

    [Header("Move Animation")]
    [SerializeField] private float moveAmplitude = 0.25f;
    [SerializeField] private float moveSpeed = 3f;

    [Header("Rotation Offset")]
    [SerializeField] private Vector3 rotationOffset = new Vector3(90f, 0f, 0f);

    private void Update()
    {
        if (player == null || target == null)
            return;

        Vector3 dir = target.position - player.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.001f)
            return;

        Vector3 targetDirection = dir.normalized;

        float pulse = Mathf.Sin(Time.time * moveSpeed) * moveAmplitude;

        transform.position =
            player.position
            + targetDirection * (forwardDistance + pulse)
            + Vector3.up * height;

        Quaternion lookRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        transform.rotation = lookRotation * Quaternion.Euler(rotationOffset);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetPlayer(Transform newPlayer)
    {
        player = newPlayer;
    }
}