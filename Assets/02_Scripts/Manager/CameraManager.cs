using UnityEngine;

public class CameraManager : GenericSingleton<CameraManager>
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 5f;

    private Vector3 offset;

    private void Start()
    {
        if (target == null)        
            return;        

        offset = transform.position - target.position; // 시작 시점 카메라 위치 유지하고 따라가기
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
    }
}
