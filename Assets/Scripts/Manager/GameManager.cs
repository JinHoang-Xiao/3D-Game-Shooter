using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Player player;

    [Header("Settings")]
    public bool friendlyFire;
    [Space]
    public bool quickStart;

    private void Awake()
    {
        instance = this;

        player = FindFirstObjectByType<Player>();
    }

    public void GameStart()
    {
        SetDefaultWeaponsForPlayer();

        //LevelGenerator.instance.InitializeGeneration();
        //Bắt đầu nhiệm vụ đã chọn trong một script LevelGenerator, sau khi quá trình tạo màn chơi hoàn tất
    }

    public void RestartScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    public void GameCompleted()
    {
        UI.instance.ShowVictoryScreenUI();
        ControlsManager.instance.controls.Character.Disable();
        player.health.currentHealth += 99999; // Player se khong chet vao may giay cuoi
    }

    public void GameOver()
    {
        TimeManager.instance.SlowMotionFor(1.5f);
        UI.instance.ShowGameOverUI();
        CameraManager.instance.ChangeCameraDistance(5);
    }

    public void SetDefaultWeaponsForPlayer()
    {
        List<Weapon_Data> newList = UI.instance.weaponSelection.SelectedWeaponData();
        player.weapon.SetDefaultWeapon(newList);
    }

}
