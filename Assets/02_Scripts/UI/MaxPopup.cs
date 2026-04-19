using UnityEngine;

public class MaxPopup : MonoBehaviour
{
    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private GameObject maxTextObject;
    [SerializeField] private float showTime = 0.5f;

    private float timer;

    private void OnEnable()
    {
        inventory.OnStoneMax += ShowMax;
    }

    private void OnDisable()
    {
        inventory.OnStoneMax -= ShowMax;
    }

    private void Update()
    {
        if (!maxTextObject.activeSelf)
            return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
            maxTextObject.SetActive(false);
    }

    private void ShowMax()
    {
        maxTextObject.SetActive(true);
        timer = showTime;
    }
}