using System;
using System.Collections;
using UnityEngine;

public class CameraManager : GenericSingleton<CameraManager>
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 5f;

    [Header("Focus Cutscene")]
    [SerializeField] private float focusMoveTime = 0.7f;
    [SerializeField] private float focusStayTime = 0.8f;

    private Vector3 offset;
    private bool isCutscenePlaying;
    private Coroutine focusRoutine;

    private void Start()
    {
        if (target == null)
            return;

        offset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        if (isCutscenePlaying)
            return;

        if (target == null)
            return;

        Vector3 desiredPos = target.position + offset;
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPos,
            smoothSpeed * Time.deltaTime
        );
    }

    #region WeaponUpgradeZone이 생길때 카메라 연출용
    public void PlayFocusRoutine(Transform focusTarget, Action onComplete = null)
    {
        if (focusTarget == null)
        {
            onComplete?.Invoke();
            return;
        }

        if (focusRoutine != null)
            StopCoroutine(focusRoutine);

        focusRoutine = StartCoroutine(CoFocusRoutine(focusTarget, onComplete));
    }

    private IEnumerator CoFocusRoutine(Transform focusTarget, Action onComplete)
    {
        isCutscenePlaying = true;

        Vector3 startPos = transform.position;

        // Rotation은 건드리지 않고, Position만 이동
        Vector3 focusPos = focusTarget.position + offset;

        yield return MoveCameraPosition(startPos, focusPos, focusMoveTime);

        yield return new WaitForSeconds(focusStayTime);

        Vector3 returnPos = target != null ? target.position + offset : startPos;

        yield return MoveCameraPosition(transform.position, returnPos, focusMoveTime);

        isCutscenePlaying = false;
        focusRoutine = null;

        onComplete?.Invoke();
    }

    private IEnumerator MoveCameraPosition(Vector3 startPos, Vector3 endPos, float duration)
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / duration);

            // 부드러운 이동
            t = t * t * (3f - 2f * t);

            transform.position = Vector3.Lerp(startPos, endPos, t);

            yield return null;
        }

        transform.position = endPos;
    }
    #endregion
}
