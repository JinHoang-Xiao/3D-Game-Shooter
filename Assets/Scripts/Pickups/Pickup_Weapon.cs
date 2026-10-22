using UnityEngine;

public class Pickup_Weapon : Interactable
{
    [SerializeField] private Weapon_Data weaponData; // Prefab của vũ khí để spawn khi nhặt
    [SerializeField] private Weapon weapon;

    [SerializeField] private BackupWeaponModel[] models;


    private bool oldWeapon;

    private void Start()
    {
        if (oldWeapon == false)
            weapon = new Weapon(weaponData);

        SetupGameObject();
    }

    public void SetupPickupWeapon(Weapon weapon, Transform transform)
    {
        oldWeapon = true;

        this.weapon = weapon;
        weaponData = weapon.weaponData;

        this.transform.position = transform.position + new Vector3(0, .75f, 0);
    }

    [ContextMenu("Update Item Model")]
    public void SetupGameObject()
    {
        gameObject.name = "Pickup_weapon - " + weaponData.weaponType.ToString();
        SetupWeaponModel();
    }


    private void SetupWeaponModel()
    {
        foreach (BackupWeaponModel model in models)
        {
            model.gameObject.SetActive(false);

            if (model.weaponType == weaponData.weaponType)
            {
                model.gameObject.SetActive(true);
                UpdateMeshAndMaterial(model.GetComponent<MeshRenderer>());
            }
        }
    }

    public override void Interaction()
    {
        weaponCotroller.PickupWeapon(weapon);

        ObjectPool.instance.ReturnObject(gameObject);
    }
}
