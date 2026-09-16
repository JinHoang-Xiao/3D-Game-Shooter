using UnityEngine;

public class WeaponVisualController : MonoBehaviour
{
    [SerializeField] private Transform[] gunTranforms;

    [SerializeField] private Transform pistol;
    [SerializeField] private Transform revolver;
    [SerializeField] private Transform autoRifle;
    [SerializeField] private Transform shotgun;
    [SerializeField] private Transform sniper;
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
    }
    private void SwitchOffGun()
    {
        for (int i = 0; i < gunTranforms.Length;i++)
        {
            gunTranforms[i].gameObject.SetActive(false);
        }
    }
}
