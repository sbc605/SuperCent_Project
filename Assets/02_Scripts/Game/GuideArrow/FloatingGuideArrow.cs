using UnityEngine;

/// <summary>
/// 오브젝트 위에서 위아래 움직임 화살표
/// </summary>
public class FloatingGuideArrow : MonoBehaviour
{
    [SerializeField] private Transform target;

    [Header("Offset")]
    [SerializeField] private Vector3 defaultOffset = new Vector3(0f, 2f, 0f);
    [SerializeField] private float extraHeight = 1.5f;

    [Header("Auto Height")]
    [SerializeField] private bool useTargetBoundsHeight = true;

    [Header("Floating")]
    [SerializeField] private float floatAmplitude = 0.3f;
    [SerializeField] private float floatSpeed = 2f;

    [Header("Rotation")]
    [SerializeField] private bool rotate = true;
    [SerializeField] private float rotateSpeed = 90f;

    private Vector3 basePosition;

    private void OnEnable()
    {
        UpdateBasePosition();
    }

    private void Update()
    {
        if (target == null)
            return;

        UpdateBasePosition();

        float y = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = basePosition + new Vector3(0f, y, 0f);

        if (rotate)
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
    }


    /// <summary>
    /// 하위의 모든 Renderer를 검사하고 extraHeight만큼 더 올려서 화살표를 배치
    /// </summary>
    private void UpdateBasePosition()
    {
        if (target == null)
            return;

        if (useTargetBoundsHeight && TryGetTargetBounds(out Bounds bounds))
        {
            transform.position = new Vector3(
                bounds.center.x,
                bounds.max.y + extraHeight,
                bounds.center.z
            );

            basePosition = transform.position;
            return;
        }

        basePosition = target.position + defaultOffset;
    }

    private bool TryGetTargetBounds(out Bounds resultBounds)
    {
        Renderer[] renderers = target.GetComponentsInChildren<Renderer>();

        if (renderers == null || renderers.Length == 0)
        {
            resultBounds = default;
            return false;
        }

        resultBounds = renderers[0].bounds;

        for (int i = 1; i < renderers.Length; i++)
        {
            resultBounds.Encapsulate(renderers[i].bounds);
        }

        return true;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        UpdateBasePosition();
    }
}