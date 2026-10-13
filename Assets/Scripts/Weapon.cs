using UnityEngine;

public enum WeaponType
{
    Pistol,
    Revolver,
    AutoRifle,
    Shotgun,
    Rifle,
}


[System.Serializable] // làm cho lớp Weapon có thể được lưu trữ và hiển thị trong Unity Inspector
public class Weapon
{
    public WeaponType weaponType; // loại vũ khí
    public int bulletsInMagazine; // số lượng đạn hiện có trong băng đạn
    public int magazineCapacity; // sức chứa tối đa của băng đạn
    public int totalReserveAmmo; // tổng số lượng đạn dự trữ

    [Range(1, 3)]
    public float reloadSpeed = 1; // nhân vật thay đạn vũ khí nhanh bao nhiêu
    [Range(1, 3)]
    public float equipmentSpeed = 1; // nhân vật mang vũ khí nhanh bao nhiêu

    [Space]
    public float fireRate = 1; // Đạn mỗi giây (bullets per second)
    private float lastShootTime;

    public bool CanShoot()
    {
        if(HaveEnoughBullets() && ReadyToFire())
        {
            bulletsInMagazine--;
            return true;
        }
        return false;
    }

    private bool ReadyToFire()
    {
        if (Time.time >lastShootTime +1 /  fireRate)
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
