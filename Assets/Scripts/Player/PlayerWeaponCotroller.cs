using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;

public class PlayerWeaponCotroller : MonoBehaviour
{
    private Player player;
    private const float REFERENCE_BULLET_SPEED = 20f; // Tham chiếu tốc độ viên đạn để điều chỉnh tốc độ bắn

    [SerializeField] private Weapon currentWeapon; // Tham chiếu đến vũ khí hiện tại của người chơi
    private bool weaponReady;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;

    

    [SerializeField] private Transform weaponHolder;


    [Header("Inventory")]
    [SerializeField] private int maxSlots = 2;
    [SerializeField] private List<Weapon> weaponSlots = new List<Weapon>();

    private void Start()
    {
        player = GetComponent<Player>();
        AssignInputEvents();

        Invoke("EquipStartingWeapon", .1f);
    }


    #region Slots Manager - Pickup/Equip/Drop/Ready Weapon

    private void EquipStartingWeapon() => EquipWeapon(0);
    
    private void EquipWeapon(int i)
    {
        SetWeaponReady(false);

        currentWeapon = weaponSlots[i];

        player.weaponVisuals.SwitchOffWeaponModels(); // Tắt hình ảnh vũ khí khi thay đổi vũ khí để không bị kỳ khi thay đổi cầm vũ khí khác
        player.weaponVisuals.PlayWeaponEquipAnimation();
    }
    public void PickupWeapon(Weapon newweapon)
    {
        if(weaponSlots.Count >= maxSlots)
        {
            Debug.Log("Inventory is full. Cannot pick up more weapons.");
            return;
        }
        weaponSlots.Add(newweapon);
        player.weaponVisuals.SwithOnBackupWeaponModels();
    }
    private void DropWeapon()
    {
        if(HasOnlyOneWeapon())
        {
            return;
        } 
        weaponSlots.Remove(currentWeapon);
        EquipWeapon(0);
    }

    public void SetWeaponReady(bool ready) => weaponReady = ready;
    public bool WeaponReady() => weaponReady;

    #endregion

    private void Shoot()
    {
        if (WeaponReady() == false)
            return;

        if(currentWeapon.CanShoot() == false)
        {
            Debug.Log("Out of ammo!");
            return;
        }

        GameObject newBullet = ObjectPool.instance.GetBullet();
        //Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));// Cách gọi lúc đầu để tạo bullets

        newBullet.transform.position = GunPoint().position;
        newBullet.transform.rotation = Quaternion.LookRotation(GunPoint().forward);

        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();

        rbNewBullet.mass = REFERENCE_BULLET_SPEED / bulletSpeed; // Điều chỉnh khối lượng của viên đạn dựa trên tốc độ bắn để duy trì quán tính
        rbNewBullet.linearVelocity = BulletDirection() * bulletSpeed;

        player.weaponVisuals.PlayFireAnimation();
    }

    private void Reload()
    {
        SetWeaponReady(false);
        player.weaponVisuals.PlayReloadAnimation();
    }

    public Vector3 BulletDirection()
    {

        Transform aim = player.aim.Aim();

        Vector3 direction = (aim.position - GunPoint().position).normalized;

        if (player.aim.CanAimPrecisly() == false && player.aim.Target() == null)
        {
            direction.y = 0;
        }

       
        return direction;
    }

    public bool HasOnlyOneWeapon() => weaponSlots.Count <= 1;

    public Weapon CurrentWeapon() => currentWeapon;

    public Weapon BackupWeapon()
    {
        foreach(Weapon weapon in weaponSlots)
        {
            if (weapon != currentWeapon)
                return weapon;
        }

        return null;
    }    

    public Transform GunPoint() => player.weaponVisuals.CurrentWeaponModel().gunPoint;

    #region Input Events
    private void AssignInputEvents()
    {
        PlayerControls controls = player.controls;

        controls.Player.Attack.performed += ctx => Shoot();

        controls.Player.EquipSlot1.performed += ctx => EquipWeapon(0);
        controls.Player.EquipSlot2.performed += ctx => EquipWeapon(1);

        controls.Player.DropCurrentWeapon.performed += ctx => DropWeapon();

        controls.Player.Reload.performed += ctx =>
        {
            if (currentWeapon.CanReload() && WeaponReady())
            {
                Reload();
            }
        };
    }

    #endregion
}

/*   private void OnDrawGizmos()
   {
       // Chỉ vẽ Gizmos khi player đã được khởi tạo (tránh lỗi NullReference)
       if (player == null || player.aim == null) return;

       if (weaponHolder != null)
       {
           Gizmos.DrawLine(weaponHolder.position, weaponHolder.position + weaponHolder.forward * 25);     Dùng để vẽ xem hướng của vũ khí và hướng nhắm có chung mục tiêu không
       }

       if (gunPoint != null)
       {
           Gizmos.color = Color.red;
           Gizmos.DrawLine(gunPoint.position, gunPoint.position + BulletDirection() * 25);
       }
   }*/
