using UnityEngine;

/// <summary>
/// GuideArrow 교체 역할
/// </summary>
public class GuideManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Transform playerTransform;

    [Header("Guide Target Sequence")]
    [SerializeField] private Transform[] guideTargets;

    [Header("Guide Arrows")]
    [SerializeField] private FloatingGuideArrow targetFloatingArrow;
    [SerializeField] private PlayerDirectionGuideArrow playerDirectionArrow;

    private int currentTargetIndex;
    private bool playerGuideActivated;
    private bool guideFinished;

    private Transform CurrentTarget
    {
        get
        {
            if (guideTargets == null)
                return null;

            if (currentTargetIndex < 0 || currentTargetIndex >= guideTargets.Length)
                return null;

            return guideTargets[currentTargetIndex];
        }
    }

    private void Awake()
    {
        if (playerController == null)
            playerController = FindObjectOfType<PlayerController>();

        if (playerTransform == null && playerController != null)
            playerTransform = playerController.transform;
    }

    private void OnEnable()
    {
        if (playerController != null)
            playerController.OnFirstMoveInput += ShowPlayerDirectionGuide;
    }

    private void OnDisable()
    {
        if (playerController != null)
            playerController.OnFirstMoveInput -= ShowPlayerDirectionGuide;
    }

    private void Start()
    {
        currentTargetIndex = 0;
        playerGuideActivated = false;
        guideFinished = false;

        ShowCurrentTargetGuide();

        if (playerDirectionArrow != null)
            playerDirectionArrow.gameObject.SetActive(false);
    }

    private void ShowCurrentTargetGuide()
    {
        Transform target = CurrentTarget;

        if (target == null)
        {
            FinishGuide();
            return;
        }

        if (targetFloatingArrow != null)
        {
            targetFloatingArrow.SetTarget(target);
            targetFloatingArrow.gameObject.SetActive(true);
        }

        if (playerDirectionArrow != null && playerGuideActivated)
        {
            playerDirectionArrow.SetPlayer(playerTransform);
            playerDirectionArrow.SetTarget(target);
            playerDirectionArrow.gameObject.SetActive(true);
        }
    }

    private void ShowPlayerDirectionGuide()
    {
        if (guideFinished)
            return;

        playerGuideActivated = true;

        Transform target = CurrentTarget;

        if (target == null)
        {
            FinishGuide();
            return;
        }

        // A 화살표는 끄지 않는다.
        // B 화살표만 켜서 A 화살표가 있는 목표 방향을 계속 가리키게 한다.
        if (playerDirectionArrow != null)
        {
            playerDirectionArrow.SetPlayer(playerTransform);
            playerDirectionArrow.SetTarget(target);
            playerDirectionArrow.gameObject.SetActive(true);
        }
    }

    public void NotifyTargetReached(Transform reachedTarget)
    {
        if (guideFinished)
            return;

        Transform target = CurrentTarget;

        if (target == null)
        {
            FinishGuide();
            return;
        }

        // 현재 목표가 아닌 다른 Trigger에 닿은 경우 무시
        if (reachedTarget != target)
            return;

        currentTargetIndex++;

        if (currentTargetIndex >= guideTargets.Length)
        {
            FinishGuide();
            return;
        }

        ShowCurrentTargetGuide();
    }

    private void FinishGuide()
    {
        guideFinished = true;

        if (targetFloatingArrow != null)
            targetFloatingArrow.gameObject.SetActive(false);

        if (playerDirectionArrow != null)
            playerDirectionArrow.gameObject.SetActive(false);

        Debug.Log("[GuideManager] 모든 가이드 순서 완료");
    }
}
