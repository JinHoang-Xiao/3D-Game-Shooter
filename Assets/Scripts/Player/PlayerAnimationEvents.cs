using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private PlayerWeaponVisuals visualController;
    private PlayerWeaponCotroller weaponController;

    private void Start()
    {
        visualController = GetComponentInParent<PlayerWeaponVisuals>();
        weaponController = GetComponentInParent<PlayerWeaponCotroller>();
    }

    public void ReloadIsOver()
    {
        visualController.MaximizeRigWeight();
        weaponController.CurrentWeapon().RefillBullets();

        weaponController.SetWeaponReady(true);
    }

    public void ReturnRig()
    {
        visualController.MaximizeRigWeight();
        visualController.MaximizeLeftHandWeight();
    }

    public void WeaponEquipingIsOver()
    {      
        weaponController.SetWeaponReady(true);
    }

    public void SwithOnWeaponModel() =>visualController.SwithOnCurrentWeaponModel();
}
