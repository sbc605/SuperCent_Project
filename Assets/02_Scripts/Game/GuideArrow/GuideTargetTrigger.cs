using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideTargetTrigger : MonoBehaviour
{
    [SerializeField] private GuideManager guideManager;

    private void Awake()
    {
        if (guideManager == null)
            guideManager = FindObjectOfType<GuideManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponentInParent<PlayerController>();

        if (player == null)
            return;

        if (guideManager != null)
            guideManager.NotifyTargetReached(transform);
    }
}
