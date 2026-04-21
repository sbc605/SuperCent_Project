using System.Collections;
using UnityEngine;

public class MaxPopup : MonoBehaviour
{
    [SerializeField] private PlayerInventory inventory;

    [Header("원본 World Canvas")]
    [SerializeField] private GameObject maxTextPrefab;

    [Header("Animation")]
    [SerializeField] private float showTime = 0.5f;
    [SerializeField] private float moveUpDistance = 1.5f;

    [Header("Camera")]
    [SerializeField] private bool faceCamera = true;

    private Coroutine popupRoutine;
    private GameObject currentPopup;

    private void Awake()
    {
        if (maxTextPrefab != null)
            maxTextPrefab.SetActive(false);
    }

    private void OnEnable()
    {
        if (inventory != null)
        {
            inventory.OnStoneMax += ShowMax;
            // inventory.OnHandcuffMax += ShowMax;
            // inventory.OnMoneyMax += ShowMax;
        }
    }

    private void OnDisable()
    {
        if (inventory != null)
        {
            inventory.OnStoneMax -= ShowMax;
            // inventory.OnHandcuffMax -= ShowMax;
            // inventory.OnMoneyMax -= ShowMax;
        }

        ClearPopup();
    }

    private void ShowMax()
    {
        if (maxTextPrefab == null)
            return;

        // 이미 팝업이 떠 있으면 새로 생성하지 않음
        if (currentPopup != null)
            return;

        popupRoutine = StartCoroutine(CoShowMax());
    }

    private IEnumerator CoShowMax()
    {
        Vector3 startPos = maxTextPrefab.transform.position;
        Quaternion startRot = maxTextPrefab.transform.rotation;
        Vector3 startScale = maxTextPrefab.transform.lossyScale;

        currentPopup = Instantiate(maxTextPrefab, startPos, startRot);
        currentPopup.SetActive(true);

        currentPopup.transform.SetParent(null, true);
        currentPopup.transform.localScale = startScale;

        CanvasGroup canvasGroup = currentPopup.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = currentPopup.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 1f;

        Vector3 endPos = startPos + Vector3.up * moveUpDistance;

        float timer = 0f;

        while (timer < showTime)
        {
            if (currentPopup == null)
                yield break;

            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / showTime);

            currentPopup.transform.position = Vector3.Lerp(startPos, endPos, t);
            canvasGroup.alpha = 1f - t;

            if (faceCamera && Camera.main != null)
            {
                Vector3 dir = currentPopup.transform.position - Camera.main.transform.position;

                if (dir.sqrMagnitude > 0.001f)
                    currentPopup.transform.rotation = Quaternion.LookRotation(dir);
            }

            yield return null;
        }

        Destroy(currentPopup);
        currentPopup = null;
        popupRoutine = null;
    }

    /// <summary>
    /// 코루틴과 복제 오브젝트 정리
    /// </summary>
    private void ClearPopup()
    {
        if (popupRoutine != null)
        {
            StopCoroutine(popupRoutine);
            popupRoutine = null;
        }

        if (currentPopup != null)
        {
            Destroy(currentPopup);
            currentPopup = null;
        }
    }
}