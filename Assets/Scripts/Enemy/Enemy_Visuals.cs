using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public enum Enemy_MeleeWeaponType { OneHand, Throw, Unarmed }
public enum Enemy_RangeWeaponType { Pistol, Revolver, Shotgun, AutoRifle, Rifle }

public class Enemy_Visuals : MonoBehaviour
{
    public GameObject currentWeaponModel { get; private set; }

    [Header("Corruption Visuals")]
    [SerializeField] private GameObject[] corruptionCrystals;
    [SerializeField] private int corruptionAmount;

    [Header("Color")]
    [SerializeField] private Texture[] colorTextures;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;

    [Header("Rig references")]
    [SerializeField] private Transform leftHandIK;
    [SerializeField] private Transform leftElbowIK;
    [SerializeField] private TwoBoneIKConstraint leftHandIKConstraint;
    [SerializeField] private MultiAimConstraint weaponAimConstraint;


    public void EnableWeaponTrail(bool enable) 
    {
        Enemy_WeaponModel currentWeaponScript = currentWeaponModel.GetComponent<Enemy_WeaponModel>();

        currentWeaponScript.EnableTrailEffect(enable); 
    }

    public void SetupLook()
    {
        SetupRandomColor();
        SetupRandomWeapon();
        SetupRandomCorruption();
    }

    private void SetupRandomCorruption()
    {
        //if (corruptionCrystals == null || corruptionCrystals.Length == 0)
        //    return;

        /* Chọn crytals random vì code dưới là random từng vị trí xuất hiện
           corruptionAmount = Random.Range(0, 9); // Hoặc từ 0 đến tất cả vị trí có (0, corruptionCrystals.Length + 1);*/

        List<int> avaliableIndexes = new List<int>();
        corruptionCrystals = CollectCorruptionCrystals();

        for (int i = 0; i < corruptionCrystals.Length; i++)
        {
            avaliableIndexes.Add(i);
            corruptionCrystals[i].SetActive(false);
        }

        for (int i = 0; i < corruptionAmount; i++)
        {
            if (avaliableIndexes.Count == 0)
                break;

            int randomIndex = Random.Range(0, avaliableIndexes.Count);
            int objectIndex = avaliableIndexes[randomIndex];

            corruptionCrystals[objectIndex].SetActive(true);
            avaliableIndexes.RemoveAt(randomIndex);
        }
    }

    private void SetupRandomWeapon()
    {
        bool thisEnemyIsMelee = GetComponent<Enemy_Melee>() != null;
        bool thisEnemyIsRange = GetComponent<Enemy_Range>() != null;

        if (thisEnemyIsRange)
        {
            currentWeaponModel = FindRangeWeaponModel();
        }

        if (thisEnemyIsMelee)
        {
            currentWeaponModel = FindMeleeWeaponModel();
        }

        currentWeaponModel.SetActive(true);

        OverrideAnimatorControllerIfCan();
    }

    private void SetupRandomColor()
    {
        int randomIndex = Random.Range(0, colorTextures.Length);

        Material newMat = new Material(skinnedMeshRenderer.material);

        newMat.mainTexture = colorTextures[randomIndex];

        skinnedMeshRenderer.material = newMat;
    }

    private GameObject FindRangeWeaponModel()
    {
        Enemy_RangeWeaponModel[] weaponModels = GetComponentsInChildren<Enemy_RangeWeaponModel>(true);
        Enemy_RangeWeaponType weaponType = GetComponent<Enemy_Range>().weaponType;
        
        foreach (var weaponModel in weaponModels)
        {
            if (weaponModel.weaponType == weaponType)
            {
                SwitchAnimationLayer(((int)weaponModel.weaponHoldType));
                SetUpLeftHandIK(weaponModel.leftHandTarget,weaponModel.leftElbowTarget);
                return weaponModel.gameObject;
            }
        }

        Debug.Log("No range weapon model found");
        return null;
    }

    private GameObject FindMeleeWeaponModel()
    {
        Enemy_WeaponModel[] weaponModels = GetComponentsInChildren<Enemy_WeaponModel>(true);
        Enemy_MeleeWeaponType weaponType = GetComponent<Enemy_Melee>().weaponType;
        List<Enemy_WeaponModel> filteredWeaponModels = new List<Enemy_WeaponModel>();

        foreach (var weaponModel in weaponModels)
        {
            if (weaponModel.weaponType == weaponType)
                filteredWeaponModels.Add(weaponModel);
        }

        int randomIndex = Random.Range(0, filteredWeaponModels.Count);
        return filteredWeaponModels[randomIndex].gameObject;
    }

    private GameObject[] CollectCorruptionCrystals()
    {
        Enemy_CorruptionCrystal[] crystalComponents = GetComponentsInChildren<Enemy_CorruptionCrystal>(true);
        GameObject[] corruptionCrystals = new GameObject[crystalComponents.Length];

        for (int i = 0; i < crystalComponents.Length; i++)
        {
            corruptionCrystals[i] = crystalComponents[i].gameObject;
        }

        return corruptionCrystals;
    }

    private void OverrideAnimatorControllerIfCan()
    {
        AnimatorOverrideController overrideController =
            currentWeaponModel.GetComponent<Enemy_WeaponModel>()?.overrideController;

        if (overrideController != null)
        {
            GetComponentInChildren<Animator>().runtimeAnimatorController = overrideController;
        }
    }

    private void SwitchAnimationLayer(int layerIndex)
    {
        Animator anim = GetComponentInChildren<Animator>();

        for (int i = 1; i < anim.layerCount; i++)
        {
            anim.SetLayerWeight(i, 0);
        }
        anim.SetLayerWeight(layerIndex, 1);
    }

    public void EnableIK(bool enableLeftHand, bool enableAim)
    {
        leftHandIKConstraint.weight = enableLeftHand ? 1 : 0;
        weaponAimConstraint.weight = enableAim ? 1 : 0;
    }

    private void SetUpLeftHandIK(Transform leftHandTarget, Transform leftElbowTarget)
    {
        leftHandIK.localPosition = leftHandTarget.localPosition;
        leftHandIK.localRotation = leftHandTarget.localRotation;

        leftElbowIK.localPosition = leftElbowTarget.localPosition;
        leftElbowIK.localRotation = leftElbowTarget.localRotation;
    }

}
