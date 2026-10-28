using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponCotroller : MonoBehaviour
{
    private Player player;
    private const float REFERENCE_BULLET_SPEED = 20f; // Tham chiếu tốc độ viên đạn để điều chỉnh tốc độ bắn


    [SerializeField] private Weapon_Data defaultWeaponData;
    [SerializeField] private Weapon currentWeapon; // Tham chiếu đến vũ khí hiện tại của người chơi
    private bool weaponReady;
    private bool isShooting;


    [Header("Bullet details")]
    [SerializeField] private float bulletImpactForce = 100;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;


    [SerializeField] private Transform weaponHolder;


    [Header("Inventory")]
    [SerializeField] private int maxSlots = 2;
    [SerializeField] private List<Weapon> weaponSlots;

    [SerializeField] private GameObject weaponPickupPrefab;


    private void Start()
    {
        player = GetComponent<Player>();
        AssignInputEvents();

        Invoke(nameof(EquipStartingWeapon), .1f);
    }

    private void Update()
    {
        if (isShooting)
            Shoot();
    }


    #region Slots Manager - Pickup/Equip/Drop/Ready Weapon

    private void EquipStartingWeapon()
    {
        weaponSlots[0] = new Weapon(defaultWeaponData);

        EquipWeapon(0);
    }

    private void EquipWeapon(int i)
    {
        if (i >= weaponSlots.Count)
        {
            return;
        }

        SetWeaponReady(false);

        currentWeapon = weaponSlots[i];

        player.weaponVisuals.SwitchOffWeaponModels(); // Tắt hình ảnh vũ khí khi thay đổi vũ khí để không bị kỳ khi thay đổi cầm vũ khí khác
        player.weaponVisuals.PlayWeaponEquipAnimation();

        CameraManager.instance.ChangeCameraDistance(currentWeapon.cameraDistance);
    }
    public void PickupWeapon(Weapon newWeapon)
    {

        if (WeaponInSlots(newWeapon.weaponType) != null)
        {
            WeaponInSlots(newWeapon.weaponType).totalReserveAmmo += newWeapon.bulletsInMagazine;
            return;
        }

        if (weaponSlots.Count >= maxSlots && newWeapon.weaponType != currentWeapon.weaponType)
        {
            int weaponIndex = weaponSlots.IndexOf(currentWeapon);

            //player.weaponVisuals.SwitchOffWeaponModels();
            weaponSlots[weaponIndex] = newWeapon;

            CreateWeaponOnTheGround();
            EquipWeapon(weaponIndex);
            return;
        }

        weaponSlots.Add(newWeapon);
        player.weaponVisuals.SwithOnBackupWeaponModels();
    }
    private void DropWeapon()
    {
        if (HasOnlyOneWeapon())
        {
            return;
        }

        CreateWeaponOnTheGround();

        weaponSlots.Remove(currentWeapon);
        EquipWeapon(0);
    }

    private void CreateWeaponOnTheGround()
    {
        GameObject droppedWeapon = ObjectPool.instance.GetObject(weaponPickupPrefab);
        droppedWeapon.GetComponent<Pickup_Weapon>()?.SetupPickupWeapon(currentWeapon, transform);
    }

    public void SetWeaponReady(bool ready) => weaponReady = ready;
    public bool WeaponReady() => weaponReady;

    #endregion

    private IEnumerator BurstFire()
    {
        SetWeaponReady(false);

        for (int i = 1; i <= currentWeapon.bulletsPerShot; i++)
        {
            FireSingleBullet();

            yield return new WaitForSeconds(currentWeapon.burstFireDelay);

            if (i >= currentWeapon.bulletsPerShot)
            {
                SetWeaponReady(true);
            }
        }
    }

    private void Shoot()
    {
        if (WeaponReady() == false)
            return;

        if (currentWeapon.CanShoot() == false)
            return;

        player.weaponVisuals.PlayFireAnimation();

        if (currentWeapon.shootType == ShootType.Single)
            isShooting = false;

        if (currentWeapon.BurstActivated() == true)
        {
            StartCoroutine(BurstFire());
            return;
        }

        FireSingleBullet();
        TriggerEnemyDodge();
    }

    private void FireSingleBullet()
    {
        currentWeapon.bulletsInMagazine--;


        GameObject newBullet = ObjectPool.instance.GetObject(bulletPrefab); //Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));// Cách gọi lúc đầu để tạo bullets

        newBullet.transform.position = GunPoint().position;
        newBullet.transform.rotation = Quaternion.LookRotation(GunPoint().forward);

        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();

        Bullet bulletScript = newBullet.GetComponent<Bullet>();
        bulletScript.BulletSetup(currentWeapon.gunDistance,bulletImpactForce);


        Vector3 bulletDirection = currentWeapon.ApplySpread(BulletDirection());

        rbNewBullet.mass = REFERENCE_BULLET_SPEED / bulletSpeed; // Điều chỉnh khối lượng của viên đạn dựa trên tốc độ bắn để duy trì quán tính
        rbNewBullet.linearVelocity = bulletDirection * bulletSpeed;
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

    public Weapon WeaponInSlots(WeaponType weaponType)
    {
        foreach (Weapon weapon in weaponSlots)
        {
            if (weapon.weaponType == weaponType)
                return weapon;
        }
        return null;
    }

    public Weapon CurrentWeapon() => currentWeapon;

    public Transform GunPoint() => player.weaponVisuals.CurrentWeaponModel().gunPoint;

    private void TriggerEnemyDodge()
    {
        Vector3 rayOrigin = GunPoint().position;
        Vector3 rayDirection = BulletDirection();

        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, Mathf.Infinity))
        {
            Enemy_Melee enemy_Melee = hit.collider.gameObject.GetComponentInParent<Enemy_Melee>();

            if (enemy_Melee != null)
            {
                enemy_Melee.ActivateDodgeRoll();
            }
        }
    }

    #region Input Events
    private void AssignInputEvents()
    {
        PlayerControls controls = player.controls;

        controls.Character.Attack.performed += ctx => isShooting = true;
        controls.Character.Attack.canceled += ctx => isShooting = false;

        controls.Character.EquipSlot1.performed += ctx => EquipWeapon(0);
        controls.Character.EquipSlot2.performed += ctx => EquipWeapon(1);
        controls.Character.EquipSlot3.performed += ctx => EquipWeapon(2);
        controls.Character.EquipSlot4.performed += ctx => EquipWeapon(3);
        controls.Character.EquipSlot5.performed += ctx => EquipWeapon(4);

        controls.Character.DropCurrentWeapon.performed += ctx => DropWeapon();

        controls.Character.Reload.performed += ctx =>
        {
            if (currentWeapon.CanReload() && WeaponReady())
            {
                Reload();
            }
        };
        controls.Character.ToggleWeaponMode.performed += ctx => currentWeapon.ToggleBurst();
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
