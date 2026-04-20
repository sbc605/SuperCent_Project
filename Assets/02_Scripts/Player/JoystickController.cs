using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// UI 핸들 이동
/// </summary>
public class JoystickController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [Header("Target")]
    [SerializeField] private PlayerController playerController;

    [Header("Joystick")]
    [SerializeField] private float maxRadius = 75f;

    [Header("UI")]
    [SerializeField] private GameObject idleGuideRoot;
    [SerializeField] private GameObject joystickRoot;
    [SerializeField] private RectTransform backgroundUI;
    [SerializeField] private RectTransform handlerUI;

    private Vector2 startPos;
    private bool isDragging;
    private bool isJoystickActivated;

    void Start()
    {
        SetIdleMode();

        if (handlerUI != null)
            handlerUI.anchoredPosition = Vector2.zero;
    }

    private void SetIdleMode()
    {
        isJoystickActivated = false;

        if (idleGuideRoot != null)
            idleGuideRoot.SetActive(true);

        if (joystickRoot != null)
            joystickRoot.SetActive(false);

        if (handlerUI != null)
            handlerUI.anchoredPosition = Vector2.zero;

        if (playerController != null)
            playerController.InputJoystick(Vector2.zero);
    }

    private void SetJoystickMode()
    {
        isJoystickActivated = true;

        if (idleGuideRoot != null)
            idleGuideRoot.SetActive(false);

        if (joystickRoot != null)
            joystickRoot.SetActive(true);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isJoystickActivated)
            SetJoystickMode();

        startPos = eventData.position;
        isDragging = true;

        handlerUI.position = Vector2.zero; // 배경 위치는 고정, 핸들만 중앙으로 초기화
        playerController.InputJoystick(Vector2.zero);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging)
            return;

        Vector2 dragDir = eventData.position - startPos;// 현재 드래그하는 방향
        Vector2 clamped = Vector2.ClampMagnitude(dragDir, maxRadius); // 실제 핸들 위치
        Vector2 normalized = clamped / maxRadius; // 플레이어 입력 이동값

        handlerUI.anchoredPosition = clamped;
        playerController.InputJoystick(normalized);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        playerController.InputJoystick(Vector2.zero);
        handlerUI.anchoredPosition = Vector2.zero;
    }
}

