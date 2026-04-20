using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 8자 모양으로 움직이는 것 구현
/// </summary>
public class JoystickIdleArrowMotion : MonoBehaviour
{
    [SerializeField] private RectTransform arrow;
    [SerializeField] private float width = 80f;
    [SerializeField] private float height = 40f;
    [SerializeField] private float speed = 2f;
    [SerializeField] private bool rotateToDirection = true;

    private float timer;
    private Vector2 previousPos;

    private void Awake()
    {
        if (arrow == null)
            arrow = GetComponent<RectTransform>();

        if (arrow != null)
            previousPos = arrow.anchoredPosition;
    }

    private void OnEnable()
    {
        timer = 0f;

        if (arrow != null)
        {
            arrow.anchoredPosition = Vector2.zero;
            previousPos = arrow.anchoredPosition;
        }
    }

    private void Update()
    {
        if (arrow == null)
            return;

        timer += Time.deltaTime * speed;

        float x = Mathf.Sin(timer) * width;
        float y = Mathf.Sin(timer * 2f) * height;

        Vector2 newPos = new Vector2(x, y);
        arrow.anchoredPosition = newPos;

        if (rotateToDirection)
        {
            Vector2 dir = newPos - previousPos;

            if (dir.sqrMagnitude > 0.001f)
            {
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                arrow.localRotation = Quaternion.Euler(0f, 0f, angle - 90f);
            }
        }

        previousPos = newPos;
    }
}
