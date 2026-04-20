using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [Header("Weapon Data")]
    [SerializeField] private WeaponData[] weaponLevels;

    [Header("Weapon Spawn Point")]
    [SerializeField] private Transform weaponSpawnPoint;

    private GameObject currentWeaponObject;

    public int CurrentLevel { get; private set; }
    public WeaponData CurrentWeaponData { get; private set; }

    private void Awake()
    {
        CurrentLevel = 0;
        EquipWeapon(CurrentLevel);
        HideWeapon();
    }

    #region Upgrade
    public bool CanUpgrade()
    {
        return CurrentLevel + 1 < weaponLevels.Length;
    }

    public int GetNextUpgradeCost()
    {
        if (!CanUpgrade())
            return 0;

        return weaponLevels[CurrentLevel + 1].upgradeCost;
    }

    public bool Upgrade()
    {
        if (!CanUpgrade())
        {
            Debug.Log("[PlayerWeapon] 이미 최대 업그레이드입니다.");
            return false;
        }

        CurrentLevel++;
        EquipWeapon(CurrentLevel);
        HideWeapon();

        Debug.Log($"[PlayerWeapon] 무기 업그레이드 완료: {CurrentWeaponData.weaponName}");
        return true;
    }
    #endregion

    #region 무기 연결
    private void EquipWeapon(int level)
    {
        if (weaponLevels == null || weaponLevels.Length == 0)
        {
            Debug.LogWarning("[PlayerWeapon] weaponLevels가 비어 있습니다.");
            return;
        }

        if (level < 0 || level >= weaponLevels.Length)
        {
            Debug.LogWarning("[PlayerWeapon] 잘못된 무기 레벨입니다.");
            return;
        }

        if (weaponSpawnPoint == null)
        {
            Debug.LogWarning("[PlayerWeapon] weaponSpawnPoint가 연결되지 않았습니다.");
            return;
        }

        CurrentWeaponData = weaponLevels[level];

        if (currentWeaponObject != null)
        {
            Destroy(currentWeaponObject);
            currentWeaponObject = null;
        }

        if (CurrentWeaponData.weaponPrefab == null)
        {
            Debug.LogWarning("[PlayerWeapon] weaponPrefab이 비어 있습니다.");
            return;
        }

        currentWeaponObject = Instantiate(
            CurrentWeaponData.weaponPrefab,
            weaponSpawnPoint.position,
            weaponSpawnPoint.rotation,
            weaponSpawnPoint
        );

        currentWeaponObject.transform.localPosition = Vector3.zero;
        currentWeaponObject.transform.localRotation = Quaternion.identity;
        currentWeaponObject.transform.localScale = Vector3.one;

        currentWeaponObject.SetActive(false);
    }

    public void ShowWeapon()
    {
        if (currentWeaponObject != null)
            currentWeaponObject.SetActive(true);
    }

    public void HideWeapon()
    {
        if (currentWeaponObject != null)
            currentWeaponObject.SetActive(false);
    }
    #endregion

    public float GetMiningPower()
    {
        if (CurrentWeaponData == null)
            return 1f;

        return CurrentWeaponData.miningPower;
    }

    public int GetDamage()
    {
        if (CurrentWeaponData == null)
            return 1;

        return CurrentWeaponData.damage;
    }

}
