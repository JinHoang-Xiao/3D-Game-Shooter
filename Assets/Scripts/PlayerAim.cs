using System;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private Player player;
    private PlayerControls controls;

    [Header("Aim Visual - Laser")]
    [SerializeField] private LineRenderer aimLaser; // component này được dùng để vẽ đường laser từ súng đến điểm nhắm, giúp người chơi thấy rõ hướng bắn.(on the weapon holder (child of player))
    


    [Header("Aim Control")]
    [SerializeField] private Transform aim;

    [SerializeField] private bool isAimingPrecisly;
    [SerializeField] private bool isLockingToTarget;


    [Header("Camera Control")]
    [SerializeField] private Transform cameraTarget;

    [Range(.5f, 1f)]
    [SerializeField] private float minCameraDistance = 1.5f;

    [Range(1f, 3f)]
    [SerializeField] private float maxCameraDistance = 4.5f;

    [Range(3f, 5f)]
    [SerializeField] private float cameraSensitivity = 5f;


    [Space]
    [SerializeField] private LayerMask aimLayerMask;
    
    private Vector2 mouseInput;
    private RaycastHit lastKnownMouseHit;

    private void Start()
    {
        player = GetComponent<Player>();
        AssignInputEvents();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            isAimingPrecisly = !isAimingPrecisly;
        }

        if(Input.GetKeyDown(KeyCode.L))
        {
            isLockingToTarget = !isLockingToTarget;
        }

        UpdateAimVisuals();
        UpdateAimPosition();
        UpdateCameraPosition();
    }
    

    private void UpdateAimVisuals()
    {
        Transform gunPoint = player.weapon.GunPoint();
        Vector3 laserDirection = player.weapon.BulletDirection();

        float laserTipLength = 0.5f;
        float gunDistance = 4f;

        Vector3 endPoint = gunPoint.position + laserDirection * gunDistance;

        if(Physics.Raycast(gunPoint.position,laserDirection,out RaycastHit hit, gunDistance))
        {
            endPoint = hit.point;
            laserTipLength = 0f;
        }

        aimLaser.SetPosition(0, gunPoint.position);
        aimLaser.SetPosition(1, endPoint);
        aimLaser.SetPosition(2, endPoint + laserDirection * laserTipLength);
    }
    private void UpdateAimPosition()
    {
        Transform target = Target();

        if(target!=null && isLockingToTarget)
        {
            aim.position = target.position;
            return;
        }

        aim.position = GetMouseHitInfo().point;
        if (!isAimingPrecisly)
        {
            aim.position = new Vector3(aim.position.x, transform.position.y + 1, aim.position.z);
        }
    }


    public Transform Target()
    {
        Transform target = null;

        if(GetMouseHitInfo().transform.GetComponent<Target>() != null)
        {
            target = GetMouseHitInfo().transform;
        }
        return target;
    }
    public Transform Aim() => aim;

    public bool CanAimPrecisly() => isAimingPrecisly;
    /*public bool CanAimPrecisly()
    {
        if (isAimingPrecisly)
        {
            return true;
        }
        return false;
    }*/

    #region Camera region
    private Vector3 DesiredCameraPosition()
    {
        /*  float actualMaxCameraDistance;
            bool movingDownward = player.movement.moveInput.y < -.5f;

            if(movingDownward)
         {
           actualMaxCameraDistance = minCameraDistance;                         // 1 cách khác để viết if else cho dòng lệnh ?
         }
            else
         {
            actualMaxCameraDistance = maxCameraDistance;
         }
        */

        float actualMaxCameraDistance = player.movement.moveInput.y < -.5f ? minCameraDistance : maxCameraDistance;

        Vector3 desiredCameraPosition = GetMouseHitInfo().point;
        Vector3 aimDirection = (desiredCameraPosition - transform.position).normalized;

        float distanceToDesiredPosition = Vector3.Distance(transform.position, desiredCameraPosition);
        float clampedDistance = Mathf.Clamp(distanceToDesiredPosition, minCameraDistance, actualMaxCameraDistance);

        desiredCameraPosition = transform.position + aimDirection * clampedDistance;
        desiredCameraPosition.y = transform.position.y + 1;

        return desiredCameraPosition;
    }
    private void UpdateCameraPosition()
    {
        cameraTarget.position = Vector3.Lerp(cameraTarget.position, DesiredCameraPosition(), cameraSensitivity * Time.deltaTime);
    }

    #endregion
    public RaycastHit GetMouseHitInfo()
    {
        Ray ray = Camera.main.ScreenPointToRay(mouseInput);
        if (Physics.Raycast(ray, out var hitInfo,Mathf.Infinity,aimLayerMask))
        {
            lastKnownMouseHit = hitInfo;
            return hitInfo;
        }
        return lastKnownMouseHit;
    }

    private void AssignInputEvents()
    {
        controls = player.controls;

        controls.Player.Aim.performed += context => mouseInput = context.ReadValue<Vector2>();
        controls.Player.Aim.canceled += context => mouseInput = Vector2.zero;
    }
}
