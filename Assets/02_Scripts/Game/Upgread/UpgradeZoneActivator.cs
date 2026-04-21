using UnityEngine;

public class UpgradeZoneActivator : MonoBehaviour
{
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private GameObject upgradeZone;

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
        // 이미 돈을 가지고 시작하는 경우 보정
        if (playerInventory != null && playerInventory.CurrentMoneyStackCount > 0)
        {
            ActivateUpgradeZone();
        }
    }

    private void ActivateUpgradeZone()
    {
        if (activated)
            return;

        activated = true;

        if (upgradeZone != null)
            upgradeZone.SetActive(true);

        Debug.Log("[UpgradeZoneActivator] UpgradeZone 활성화");
    }
}
