
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
    public int bulletInMagazine; // số lượng đạn hiện có trong băng đạn
    public int magazineCapacity; // sức chứa tối đa của băng đạn
    public int totalReserveAmmo; // tổng số lượng đạn dự trữ


    public bool CanShoot()
    {
        return HaveEnoughBullets();
    }

    private bool HaveEnoughBullets()
    {
        if (bulletInMagazine > 0)
        {
            bulletInMagazine--; // giảm số lượng đạn khi bắn
            return true; // vũ khí có thể bắn nếu còn đạn
        }
        return false; // vũ khí không thể bắn nếu hết đạn
    }
    public bool CanReload()
    {
        if(bulletInMagazine == magazineCapacity) // kiểm tra nếu băng đạn đã đầy
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
        totalReserveAmmo += bulletInMagazine; // thêm số lượng đạn còn lại trong băng đạn vào tổng số lượng đạn dự trữ

        int bulletsToReload = magazineCapacity;

        if(bulletsToReload > totalReserveAmmo)
        {
            bulletsToReload = totalReserveAmmo;
        }

        totalReserveAmmo -= bulletsToReload;
        bulletInMagazine = bulletsToReload;

        if (totalReserveAmmo < 0)
        {
            totalReserveAmmo = 0; // đảm bảo số lượng đạn dự trữ không âm
        }
    }
}
