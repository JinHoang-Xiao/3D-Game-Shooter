using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Timer Mission", menuName = "Missions/Timer - Mission")]

public class Mission_Timer : Mission
{
    public float time;
    private float currentTime;


    public override void StartMission()
    {
        currentTime = time;
    }

    public override void UpdateMission()
    {
        currentTime -= Time.deltaTime;

        if (currentTime < 0)
        {
            Debug.Log("GAME OVER");
        }

        string timeText = System.TimeSpan.FromSeconds(currentTime).ToString("mm':'ss");

        string missionText = "reach the assembly point before the plane takes off.";
        string missionDetails = "time left: " + timeText;

        UI.instance.inGameUI.UpdateMissionInfo(missionText, missionDetails);
    }

    public override bool MissionCompleted()
    {
        return currentTime > 0;
    }
}
