using UnityEngine;

public enum EquipType { SideEquipAnimation, BackEquipAnimation };
public enum HoldType { CommonHold = 1, LowHold, HighHold }; 

public class WeaponModel : MonoBehaviour
{
    public WeaponType weaponType; // loại vũ khí
    public EquipType EquipAnimationType; // loại cầm vũ khí (cầm bên hông hoặc cầm sau lưng)
    public HoldType holdType; // loại cầm vũ khí (cầm thấp, cầm cao, cầm bình thường)

    public Transform gunPoint;
    public Transform holdPoint;
}
