using System.Collections;
using UnityEngine;

/// <summary>
/// 플레이어가 닿아있는 동안 $1씩 지불
/// </summary>
public class WeaponUpgradeZone : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private PlayerWeapon targetWeapon;

    [Header("Payment")]
    [SerializeField] private float payInterval = 0.05f;
    [SerializeField] private int payAmountPerTick = 1;

    [Header("UI")]
    [SerializeField] private WeaponUpgradeZoneUI upgradeUI;

    [Header("Option")]
    [SerializeField] private bool deactivateAfterUpgrade = false;

    private PlayerInventory currentInventory;
    private PlayerWeapon currentWeapon;

    private Coroutine payRoutine;

    private int totalCost;
    private int remainCost;

    private bool initialized;

    private void Start()
    {
        InitializeCostFromTarget();
        RefreshUI();
    }

    private void InitializeCostFromTarget()
    {
        if (targetWeapon == null)
        {
            Debug.LogWarning("[WeaponUpgradeZone] targetWeapon이 연결되지 않았습니다.");
            return;
        }

        if (!targetWeapon.CanUpgrade())
        {
            totalCost = 0;
            remainCost = 0;
            initialized = false;
            return;
        }

        totalCost = targetWeapon.GetNextUpgradeCost();
        remainCost = totalCost;
        initialized = true;
    }

    private void RefreshUI()
    {
        if (upgradeUI == null)
            return;

        if (targetWeapon != null && !targetWeapon.CanUpgrade())
        {
            upgradeUI.SetMax();
            return;
        }

        upgradeUI.Show(remainCost);
    }

    #region Trigger

    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory inventory = other.GetComponent<PlayerInventory>();
        if (inventory == null)
            inventory = other.GetComponentInParent<PlayerInventory>();

        PlayerWeapon weapon = other.GetComponent<PlayerWeapon>();
        if (weapon == null)
            weapon = other.GetComponentInParent<PlayerWeapon>();

        if (inventory == null || weapon == null)
            return;

        if (targetWeapon != null && weapon != targetWeapon)
            return;

        if (!weapon.CanUpgrade())
        {
            if (upgradeUI != null)
                upgradeUI.SetMax();

            Debug.Log("[WeaponUpgradeZone] 이미 최대 업그레이드입니다.");
            return;
        }

        currentInventory = inventory;
        currentWeapon = weapon;

        if (!initialized)
        {
            totalCost = weapon.GetNextUpgradeCost();
            remainCost = totalCost;
            initialized = true;
        }

        RefreshUI();

        if (payRoutine == null)
            payRoutine = StartCoroutine(PayRoutine());
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerInventory inventory = other.GetComponent<PlayerInventory>();
        if (inventory == null)
            inventory = other.GetComponentInParent<PlayerInventory>();

        if (inventory != currentInventory)
            return;

        StopPayment();

        currentInventory = null;
        currentWeapon = null;

        RefreshUI();
    }

    #endregion


    private IEnumerator PayRoutine()
    {
        while (currentInventory != null && currentWeapon != null && remainCost > 0)
        {
            if (currentInventory.TotalMoney <= 0)
            {
                Debug.Log("[WeaponUpgradeZone] 돈이 부족해서 결제 중단");
                break;
            }

            Sfx.Play("item_clip");
            int payValue = Mathf.Min(payAmountPerTick, remainCost);

            bool paid = currentInventory.TrySpendMoneyValue(payValue);

            if (!paid)
                break;

            remainCost -= payValue;

            RefreshUI();

            yield return new WaitForSeconds(payInterval);
        }

        if (remainCost <= 0)
            CompleteUpgrade();

        payRoutine = null;
    }

    private void CompleteUpgrade()
    {
        if (currentWeapon == null)
            return;

        currentWeapon.Upgrade();

        Debug.Log($"[WeaponUpgradeZone] 무기 업그레이드 완료. 비용: ${totalCost}");

        ResetCostForNextUpgrade();

        // if (deactivateAfterUpgrade)
        //     gameObject.SetActive(false);
    }

    /// <summary>
    /// 업그레이드가 끝나면 다음 비용으로 초기화
    /// </summary>
    private void ResetCostForNextUpgrade()
    {
        if (currentWeapon == null)
            return;

        if (!currentWeapon.CanUpgrade())
        {
            totalCost = 0;
            remainCost = 0;
            initialized = false;

            if (upgradeUI != null)
                upgradeUI.SetMax();

            return;
        }

        totalCost = currentWeapon.GetNextUpgradeCost();
        remainCost = totalCost;
        initialized = true;

        RefreshUI();
    }

    private void StopPayment()
    {
        if (payRoutine != null)
        {
            StopCoroutine(payRoutine);
            payRoutine = null;
        }
    }
}
