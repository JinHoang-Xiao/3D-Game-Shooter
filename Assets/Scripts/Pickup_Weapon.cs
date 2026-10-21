using UnityEngine;

public class Pickup_Weapon : Interactable
{
    private PlayerWeaponCotroller weaponCotroller;
    [SerializeField] private Weapon_Data weaponData; // Prefab của vũ khí để spawn khi nhặt

    [SerializeField] private BackupWeaponModel[] models;


    private void Start()
    {
        UpdateGameObject();
    }

    [ContextMenu("Update Item Model")]
    public void UpdateGameObject()
    {
        gameObject.name = "Pick weapon - " + weaponData.weaponType.ToString();
        UpdateItemModel();
    }


    public void UpdateItemModel()
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
        weaponCotroller.PickupWeapon(weaponData);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (weaponCotroller == null)
        {
            weaponCotroller = other.GetComponent<PlayerWeaponCotroller>();
        }
    }
}
