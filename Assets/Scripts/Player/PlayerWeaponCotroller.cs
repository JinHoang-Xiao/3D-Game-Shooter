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
    
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform gunPoint;

    

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


    #region Slots Manager - Pickup/Equip/Drop Weapon

    private void EquipStartingWeapon() => EquipWeapon(0);
    
    private void EquipWeapon(int i)
    {
        currentWeapon = weaponSlots[i];

        player.weaponVisuals.SwitchOffWeaponModels();
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
    #endregion

    private void Shoot()
    {
        if(currentWeapon.CanShoot() == false)
        {
            Debug.Log("Out of ammo!");
            return;
        }

        GameObject newBullet = ObjectPool.instance.GetBullet();
        //Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));// Cách gọi lúc đầu để tạo bullets

        newBullet.transform.position = gunPoint.position;
        newBullet.transform.rotation = Quaternion.LookRotation(gunPoint.forward);

        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();

        rbNewBullet.mass = REFERENCE_BULLET_SPEED / bulletSpeed; // Điều chỉnh khối lượng của viên đạn dựa trên tốc độ bắn để duy trì quán tính
        rbNewBullet.linearVelocity = BulletDirection() * bulletSpeed;

        GetComponentInChildren<Animator>().SetTrigger("Fire");
    }

    public Vector3 BulletDirection()
    {

        Transform aim = player.aim.Aim();

        Vector3 direction = (aim.position - gunPoint.position).normalized;

        if (player.aim.CanAimPrecisly() == false && player.aim.Target() == null)
        {
            direction.y = 0;
        }

        //weaponHolder.LookAt(aim);
        //gunPoint.LookAt(aim); TODO: Xem xét việc sử dụng LookAt để hướng vũ khí và điểm bắn về phía mục tiêu, nhưng cần kiểm tra kỹ lưỡng để tránh các vấn đề về xoay không mong muốn.

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

    public Transform GunPoint() => gunPoint;

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
            if (currentWeapon.CanReload())
            {
                player.weaponVisuals.PlayReloadAnimation();
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
