using System;
using UnityEditor.Search;
using UnityEngine;

public class PlayerWeaponCotroller : MonoBehaviour
{
    private Player player;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform gunPoint;

    [SerializeField] private Transform weaponHolder;

    private void Start()
    {
        player = GetComponent<Player>();
        player.controls.Player.Attack.performed += ctx => Shoot();
    }
    private void Shoot()
    {
        

        GameObject newBullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));

        newBullet.GetComponent<Rigidbody>().linearVelocity = BulletDirection() * bulletSpeed;

        Destroy(newBullet, 8f);

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
    
    public Transform GunPoint() => gunPoint;

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
}
