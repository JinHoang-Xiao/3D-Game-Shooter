using UnityEngine;

public enum WeaponType
{
    Pistol,
    Revolver,
    AutoRifle,
    Shotgun,
    Rifle,
}

public enum ShootType
{
    Single,
    Auto
}


[System.Serializable] // làm cho lớp Weapon có thể được lưu trữ và hiển thị trong Unity Inspector
public class Weapon
{
    public WeaponType weaponType; // loại vũ khí

    #region Regular mode variables
    public ShootType shootType;
    public int bulletsPerShot { get; private set; }

    private float defaultFireRate;
    private float fireRate = 1; // Đạn mỗi giây (bullets per second)
    private float lastShootTime;
    #endregion

    #region Burst mode variables
    private bool burstAvalible;
    public bool burstActive;

    private int burstBulletsPerShot;
    private float burstFireRate;
    public float burstFireDelay { get; private set; }
    #endregion

    [Header("Magazine details")]
    public int bulletsInMagazine; // số lượng đạn hiện có trong băng đạn
    public int magazineCapacity; // sức chứa tối đa của băng đạn
    public int totalReserveAmmo; // tổng số lượng đạn dự trữ

    #region Weapon generic info variables
    public float reloadSpeed { get; private set; } // nhân vật thay đạn vũ khí nhanh bao nhiêu
    public float equipmentSpeed { get; private set; } // nhân vật mang vũ khí nhanh bao nhiêu
    public float gunDistance { get; private set; }
    public float cameraDistance { get; private set; }
    #endregion

    #region Weapon spread variables
    [Header("Spread")]
    private float baseSpread = 1;
    private float maximumSpread = 3;
    private float currentSpread = 2;

    private float spreadIncreaseRate = .15f;

    private float lastSpreadUpdateTime;
    private float spreadCoolDown = 1;
    #endregion

    public Weapon(Weapon_Data weaponData)
    {
        bulletsInMagazine = weaponData.bulletsInMagazine;
        magazineCapacity = weaponData.magazineCapacity;
        totalReserveAmmo = weaponData.totalReserveAmmo;


        fireRate = weaponData.fireRate;
        weaponType = weaponData.weaponType;


        bulletsPerShot = weaponData.bulletsPerShot;
        shootType = weaponData.shootType;


        burstAvalible = weaponData.burstAvalible;
        burstActive = weaponData.burstActive;
        burstBulletsPerShot = weaponData.burstBulletsPerShot;
        burstFireRate = weaponData.burstFireRate;
        burstFireDelay = weaponData.burstFireDelay;


        baseSpread = weaponData.baseSpread;
        maximumSpread = weaponData.maxSpread;
        spreadIncreaseRate = weaponData.spreadIncreaseRate;


        reloadSpeed = weaponData.reloadSpeed;
        equipmentSpeed = weaponData.equipmentSpeed;
        gunDistance = weaponData.gunDistance;
        cameraDistance = weaponData.cameraDistance;


        defaultFireRate = fireRate;
    }


    #region Spread Methods

    public Vector3 ApplySpread(Vector3 originalDirection)
    {
        UpdateSpread();

        float randomizedValue = Random.Range(-currentSpread, currentSpread);

        Quaternion spreadRotation = Quaternion.Euler(randomizedValue, randomizedValue, randomizedValue);

        return spreadRotation * originalDirection;
    }

    private void UpdateSpread()
    {

        if (Time.time > lastSpreadUpdateTime + spreadCoolDown)
        {
            currentSpread = baseSpread;
        }
        else
        {
            IncreaseSpread();
        }
        lastSpreadUpdateTime = Time.time;
    }

    private void IncreaseSpread()
    {
        currentSpread = Mathf.Clamp(currentSpread + spreadIncreaseRate, baseSpread, maximumSpread);
    }

    #endregion


    #region Burst Methods

    public bool BurstActivated()
    {
        if (weaponType == WeaponType.Shotgun)
        {
            burstFireDelay = 0;
            return true;
        }

        return burstActive;
    }

    public void ToggleBurst()
    {
        if (burstAvalible == false)
        {
            return;
        }

        burstActive = !burstActive;

        if (burstActive)
        {
            bulletsPerShot = burstBulletsPerShot;
            fireRate = burstFireRate;
        }
        else
        {
            bulletsPerShot = 1;
            fireRate = defaultFireRate;
        }
    }


    #endregion

    public bool CanShoot() => HaveEnoughBullets() && ReadyToFire();

    private bool ReadyToFire()
    {
        if (Time.time > lastShootTime + 1 / fireRate)
        {
            lastShootTime = Time.time;
            return true;
        }
        return false;
    }



    #region Reload methods
    public bool CanReload()
    {
        if (bulletsInMagazine == magazineCapacity) // kiểm tra nếu băng đạn đã đầy
        {
            return false; // không thể nạp đạn nếu băng đạn đã đầy
        }

        if (totalReserveAmmo > 0)
        {
            return true; // có thể nạp đạn nếu còn đạn dự trữ và băng đạn chưa đầy
        }
        return false;// không thể nạp đạn nếu hết đạn dự trữ hoặc băng đạn đã đầy
    }
    public void RefillBullets()
    {
        totalReserveAmmo += bulletsInMagazine; // thêm số lượng đạn còn lại trong băng đạn vào tổng số lượng đạn dự trữ

        int bulletsToReload = magazineCapacity;

        if (bulletsToReload > totalReserveAmmo)
        {
            bulletsToReload = totalReserveAmmo;
        }

        totalReserveAmmo -= bulletsToReload;
        bulletsInMagazine = bulletsToReload;

        if (totalReserveAmmo < 0)
        {
            totalReserveAmmo = 0; // đảm bảo số lượng đạn dự trữ không âm
        }
    }
    private bool HaveEnoughBullets() => bulletsInMagazine > 0;
    #endregion
}
