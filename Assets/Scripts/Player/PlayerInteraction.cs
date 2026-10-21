using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public List<Interactable> interactables;
    private Interactable closestInteractable;

    private void Start()
    {
        Player player = GetComponent<Player>();
        player.controls.Character.Interaction.performed += context => InteractWithClosest();
    }

    private void InteractWithClosest()
    {
        closestInteractable?.Interaction();
    }

    public void UpdateClosestInteractable()
    {
        closestInteractable?.HighLightActive(false);

        closestInteractable = null;
        float closestDistance = float.MaxValue;

        foreach (Interactable  interactable in interactables)
        {
            float distance = Vector3.Distance(transform.position, interactable.transform.position);

            if(distance < closestDistance)
            {
                closestDistance = distance;
                closestInteractable = interactable;
            }
        }

        closestInteractable?.HighLightActive(true);
    }
}
