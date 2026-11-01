using System.Collections.Generic;
using UnityEngine;

public enum Enemy_MeleeWeaponType { OneHand, Throw, Unarmed }

public class Enemy_Visuals : MonoBehaviour
{

    [Header("Weapon Visuals")]
    [SerializeField] private Enemy_WeaponModel[] weaponModels;
    [SerializeField] private Enemy_MeleeWeaponType weaponType;
    public GameObject currentWeaponModel { get; private set; }


    [Header("Corruption Visuals")]
    [SerializeField] private GameObject[] corruptionCrystals;
    [SerializeField] private int corruptionAmount;


    [Header("Color")]
    [SerializeField] private Texture[] colorTextures;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;

    private void Awake()
    {
        weaponModels = GetComponentsInChildren<Enemy_WeaponModel>(true);
        ColectCorruptionCrystals();
    }

    public void EnableWeaponTrail(bool enable)
    {
        Enemy_WeaponModel currentWeaponScript = currentWeaponModel.GetComponent<Enemy_WeaponModel>();

        currentWeaponScript.EnableTrailEffect(enable);
    }

    public void SetupWeaponType(Enemy_MeleeWeaponType type) => weaponType = type;

    public void SetupLook()
    {
        SetupRandomColor();
        SetupRandomWeapon();
        SetupRandomCorruption();
    }

    private void SetupRandomCorruption()
    {
        if (corruptionCrystals == null || corruptionCrystals.Length == 0)
            return;

        // Chọn ngẫu nhiên số crystal: từ 0 đến tổng số vị trí.
        corruptionAmount = Random.Range(0, corruptionCrystals.Length + 1);

        List<int> avaliableIndexes = new List<int>();

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
        foreach (var weaponModel in weaponModels)
        {
            weaponModel.gameObject.SetActive(false);
        }

        List<Enemy_WeaponModel> filteredWeaponModels = new List<Enemy_WeaponModel>();

        foreach (var weaponModel in weaponModels)
        {
            if (weaponModel.weaponType == weaponType)
                filteredWeaponModels.Add(weaponModel);
        }

        int randomIndex = Random.Range(0, filteredWeaponModels.Count);

        currentWeaponModel = filteredWeaponModels[randomIndex].gameObject;
        currentWeaponModel.SetActive(true);

        OverrideAnimatorControllerIfCan();
    }

    private void OverrideAnimatorControllerIfCan()
    {
        AnimatorOverrideController overrideController =
            currentWeaponModel.GetComponent<Enemy_WeaponModel>().overrideController;

        if (overrideController != null)
        {
            GetComponentInChildren<Animator>().runtimeAnimatorController = overrideController;
        }
    }

    private void SetupRandomColor()
    {
        int randomIndex = Random.Range(0, colorTextures.Length);

        Material newMat = new Material(skinnedMeshRenderer.material);

        newMat.mainTexture = colorTextures[randomIndex];

        skinnedMeshRenderer.material = newMat;
    }

    private void ColectCorruptionCrystals()
    {
        Enemy_CorruptionCrystal[] crystalComponents = GetComponentsInChildren<Enemy_CorruptionCrystal>(true);
        corruptionCrystals = new GameObject[crystalComponents.Length];

        for (int i = 0; i < crystalComponents.Length; i++)
        {
            corruptionCrystals[i] = crystalComponents[i].gameObject;
        }
    }
}
