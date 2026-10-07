
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
    public int ammo; // số lượng đạn hiện tại của vũ khí
    public int maxAmmo; // số lượng đạn tối đa của vũ khí


    public bool CanShoot()
    {
        return HaveEnoughBullets();
    }

    private bool HaveEnoughBullets()
    {
        if (ammo > 0)
        {
            ammo--; // giảm số lượng đạn khi bắn
            return true; // vũ khí có thể bắn nếu còn đạn
        }
        return false; // vũ khí không thể bắn nếu hết đạn
    }
}
