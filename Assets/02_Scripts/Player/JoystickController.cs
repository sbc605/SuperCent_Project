using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// UI 핸들 이동
/// </summary>
public class JoystickController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float maxRadius = 75f;

    [Header("UI")]
    [SerializeField] private RectTransform backgroundUI;
    [SerializeField] private RectTransform handlerUI;

    private Vector2 startPos;
    private bool isDragging;

    void Start()
    {
        handlerUI.anchoredPosition = Vector2.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        startPos = eventData.position;
        isDragging = true;
        handlerUI.position = Vector2.zero; // 배경 위치는 고정, 핸들만 중앙으로 초기화
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

