using UnityEngine;

public class WeaponVisualController : MonoBehaviour
{
    [SerializeField] private Transform[] gunTranforms;

    [SerializeField] private Transform pistol;
    [SerializeField] private Transform revolver;
    [SerializeField] private Transform autoRifle;
    [SerializeField] private Transform shotgun;
    [SerializeField] private Transform sniper;

    private Transform currentGun;

    [Header("Left Hand IK")]
    [SerializeField] private Transform leftHand;

    private void Start()
    {
        SwithOn(pistol);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwithOn(pistol);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwithOn(revolver);
        }
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwithOn(autoRifle);
        }
        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwithOn(shotgun);
        }
        if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwithOn(sniper);
        }
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
}
