using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public PlayerControls controls { get; private set;}
    public PlayerAim aim {get; private set;}

    public PlayerMovement movement { get; private set; }
    public PlayerWeaponCotroller weapon { get; private set; }
    public PlayerWeaponVisuals weaponVisuals { get; private set; }


    private void Awake()
    {
        controls = new PlayerControls();
        aim = GetComponent<PlayerAim>();
        movement = GetComponent<PlayerMovement>();
        weapon = GetComponent<PlayerWeaponCotroller>();
        weaponVisuals = GetComponent<PlayerWeaponVisuals>();

    }

    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }
}
