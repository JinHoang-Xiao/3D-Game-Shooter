using UnityEngine;

public class Item_Pickup : MonoBehaviour
{

    [SerializeField] private Weapon weapon; // Prefab của vũ khí để spawn khi nhặt

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<PlayerWeaponCotroller>()?.PickupWeapon(weapon); 
        // Sử dụng toán tử null-conditional để kiểm tra và gọi phương thức PickupWeapon nếu PlayerWeaponCotroller tồn tại


        /*
           if(other.GetComponent<PlayerWeaponCotroller>() != null)
        {
            PlayerWeaponCotroller playerWeaponController = other.GetComponent<PlayerWeaponCotroller>();
            playerWeaponController.PickupWeapon(weapon);
            Destroy(gameObject); // Xóa vật phẩm khỏi thế giới sau khi nhặt
        }
        */
    }
}
