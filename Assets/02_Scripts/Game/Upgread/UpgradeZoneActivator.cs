using UnityEngine;

public class UpgradeZoneActivator : MonoBehaviour
{
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private GameObject upgradeZone;

    [Header("Cutscene")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Transform cameraFocusTarget;

    private bool activated;

    private void Awake()
    {
        if (upgradeZone != null)
            upgradeZone.SetActive(false);
    }

    private void OnEnable()
    {
        if (playerInventory != null)
            playerInventory.OnFirstMoneyAdded += ActivateUpgradeZone;
    }

    private void OnDisable()
    {
        if (playerInventory != null)
            playerInventory.OnFirstMoneyAdded -= ActivateUpgradeZone;
    }

    private void Start()
    {
        if (playerController == null && playerInventory != null)
            playerController = playerInventory.GetComponent<PlayerController>();

        // 이미 돈을 가지고 시작하는 경우 보정
        if (playerInventory != null && playerInventory.CurrentMoneyStackCount > 0)
            ActivateUpgradeZone();
    }

    private void ActivateUpgradeZone()
    {
        if (activated)
            return;

        activated = true;

        if (upgradeZone != null)
            upgradeZone.SetActive(true);

        if (cameraFocusTarget == null && upgradeZone != null)
            cameraFocusTarget = upgradeZone.transform;

        PlayUpgradeZoneCameraCutscene();


        Debug.Log("[UpgradeZoneActivator] UpgradeZone 활성화");
    }

    /// <summary>
    /// 처음 WeaponUpgradeZone생길 때 카메라 컷신
    /// </summary>
    private void PlayUpgradeZoneCameraCutscene()
    {
        if (playerController != null)
            playerController.SetInputLocked(true);

        if (CameraManager.Instance == null)
        {
            if (playerController != null)
                playerController.SetInputLocked(false);

            return;
        }

        CameraManager.Instance.PlayFocusRoutine(cameraFocusTarget, () =>
        {
            if (playerController != null)
                playerController.SetInputLocked(false);
        });
    }
}
