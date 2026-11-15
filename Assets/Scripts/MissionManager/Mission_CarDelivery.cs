using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Car delivery - Mission", menuName = "Missions/Car delivery - Mission")]

public class Mission_CarDelivery : Mission
{
    private bool carWasDelivered;
    public override void StartMission()
    {
        FindFirstObjectByType<MissionObject_CarDeliveryZone>(FindObjectsInactive.Include).gameObject.SetActive(true);

        string missionText = "Find a vehicle that is still in working order.";
        string missionDetails = "Drive to the assembly point.";

        UI.instance.inGameUI.UpdateMissionInfo(missionText, missionDetails);

        carWasDelivered = false;
        MissionObject_CarToDeliver.OnCarDelivery += CarDeliveryCompleted;

        Car[] cars = FindObjectsByType<Car>(FindObjectsSortMode.None);

        foreach (Car car in cars)
        {
            car.AddComponent<MissionObject_CarToDeliver>();
        }

    }

    public override bool MissionCompleted()
    {
        return carWasDelivered;
    }

    private void CarDeliveryCompleted()
    {
        carWasDelivered = true;
        MissionObject_CarToDeliver.OnCarDelivery -= CarDeliveryCompleted;

        UI.instance.inGameUI.UpdateMissionInfo("Drive to the assembly point.");
    }

}
