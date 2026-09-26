using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponVisualController : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private Transform[] gunTranforms;

    [SerializeField] private Transform pistol;
    [SerializeField] private Transform revolver;
    [SerializeField] private Transform autoRifle;
    [SerializeField] private Transform shotgun;
    [SerializeField] private Transform sniper;

    private Transform currentGun;


    [Header("Rig")]
    [SerializeField] private float rigIncreaseStep;
    private bool rigShouldBeIncreased;

    [Header("Left Hand IK")]
    [SerializeField] private Transform leftHand;

    private Rig rig;
    private void Start()
    {
        SwithOn(pistol);
        anim = GetComponentInChildren<Animator>();
        rig = GetComponentInChildren<Rig>();
    }

    private void Update()
    {
        CheckWeaponSwitch();
        if (Input.GetKeyDown(KeyCode.R) && anim != null && rig != null)
        {
            anim.SetTrigger("Reload");
            rig.weight = 0.15f;
        }
        
        if (rigShouldBeIncreased && rig != null)
        {
            rig.weight = Mathf.MoveTowards(rig.weight, 1f, rigIncreaseStep * Time.deltaTime);
            if (Mathf.Approximately(rig.weight, 1f))
            {
                rigShouldBeIncreased = false;
            }
        }
    }

    public void ReturnRigWeightToOne()
    {
        if (rig == null)
        {
            rig = GetComponentInChildren<Rig>();
        }

        if (rig == null)
        {
            return;
        }

        rigShouldBeIncreased = true;
    }

    private void SwithOn(Transform gunTransform)
    {
        SwitchOffGun();
        gunTransform.gameObject.SetActive(true);
        currentGun = gunTransform;

        AttachLeftHand();
    }
    private void SwitchOffGun()
    {
        for (int i = 0; i < gunTranforms.Length;i++)
        {
            gunTranforms[i].gameObject.SetActive(false);
        }
    }
    private void AttachLeftHand()
    {
        Transform targetTransform = currentGun.GetComponentInChildren<LeftHandTargetTransform>().transform;

        leftHand.localPosition = targetTransform.localPosition;
        leftHand.localRotation = targetTransform.localRotation;
    }
    private void SwitchAnimationLayer(int layerIndex)
    {
        for (int i = 1; i < anim.layerCount; i++)
        {
                anim.SetLayerWeight(i, 0);
        }
        anim.SetLayerWeight(layerIndex, 1);
    }



    private void CheckWeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwithOn(pistol);
            SwitchAnimationLayer(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwithOn(revolver);
            SwitchAnimationLayer(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwithOn(autoRifle);
            SwitchAnimationLayer(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwithOn(shotgun);
            SwitchAnimationLayer(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwithOn(sniper);
            SwitchAnimationLayer(3);
        }
    }
}
