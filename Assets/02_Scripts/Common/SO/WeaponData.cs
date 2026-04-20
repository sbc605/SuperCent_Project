using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public WeaponType weaponType;
    public string weaponName;

    [Header("Visual")]
    public GameObject weaponPrefab;

    [Header("Upgrade")]
    public int upgradeCost = 20;

    [Header("Stats")]
    public float miningPower = 1f;
    public int damage = 1;
}