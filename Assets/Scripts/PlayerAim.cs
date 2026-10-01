using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private Player player;
    private PlayerControls controls;


    [SerializeField] private Transform aim;
    [SerializeField] private LayerMask aimLayerMask;
    private Vector3 lookingDirection;
    private Vector2 aimInput;

    private void Start()
    {
        player = GetComponent<Player>();
        AssignInputEvents();
    }

    private void Update()
    {
        aim.position = new Vector3(GetmousePosition().x, transform.position.y, GetmousePosition().z);
    }

    public Vector3 GetmousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(aimInput);
        if (Physics.Raycast(ray, out var hitInfo,Mathf.Infinity,aimLayerMask))
        {
            return hitInfo.point;
        }
        return Vector3.zero;
    }
    private void AssignInputEvents()
    {
        controls = player.controls;

        controls.Player.Aim.performed += context => aimInput = context.ReadValue<Vector2>();
        controls.Player.Aim.canceled += context => aimInput = Vector2.zero;
    }
}
