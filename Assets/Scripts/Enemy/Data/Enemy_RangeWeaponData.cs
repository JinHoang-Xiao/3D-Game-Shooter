using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Enemy Data/Range Weapon Data")]

public class Enemy_RangeWeaponData : ScriptableObject
{
    [Header("Weapon Details")]
    public Enemy_RangeWeaponType weaponType;
    public float fireRate = 1f; // bullets per second

    public int minBulletsPerAttack = 1;
    public int maxBulletsPerAttack = 2;

    public float minWeaponCooldown = 2;
    public float maxWeaponCooldown = 3;


    [Header("Bullet Details")]
    public int bulletDamage;

    [Space]
    public float bulletSpeed = 20;
    public float weaponSpread = .1f;



    public int GetBulletsPerAttack() => Random.Range(minBulletsPerAttack, maxBulletsPerAttack + 1); // +1 vì giá trị tối đa của Random.Range không bao gồm chính nó.
    public float GetWeaponCooldown() => Random.Range(minWeaponCooldown, maxWeaponCooldown);

    public Vector3 ApplyWeaponSpread(Vector3 originalDirection)
    {

        float randomizedValue = Random.Range(-weaponSpread, weaponSpread);

        Quaternion spreadRotation = Quaternion.Euler(randomizedValue, randomizedValue / 2, randomizedValue);

        return spreadRotation * originalDirection;
    }
}
