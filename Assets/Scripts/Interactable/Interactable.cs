using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected Player_WeaponCotroller weaponCotroller;
    [SerializeField] protected MeshRenderer mesh;


    [SerializeField] private Material highlightMaterial;
    [SerializeField] protected Material defaultMaterial;

    private void Start()
    {
        if (mesh == null)
            mesh = GetComponentInChildren<MeshRenderer>();

        defaultMaterial = mesh.sharedMaterial;
    }

    protected void UpdateMeshAndMaterial(MeshRenderer newMesh)
    {
        mesh = newMesh;
        defaultMaterial = newMesh.sharedMaterial;
    }

    public virtual void Interaction()
    {
        Debug.Log("Interacted with" + gameObject.name);
    }

    public void HighLightActive(bool active)
    {
        if (active)
        {
            mesh.material = highlightMaterial;
        }
        else
            mesh.material = defaultMaterial;
    }



    protected virtual void OnTriggerEnter(Collider other)
    {
        if (weaponCotroller == null)
        {
            weaponCotroller = other.GetComponent<Player_WeaponCotroller>();
        }

        Player_Interaction playerInteraction = other.GetComponent<Player_Interaction>();

        if (playerInteraction == null)
            return;
       
        playerInteraction.GetInteractables().Add(this);
        playerInteraction.UpdateClosestInteractable();
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        Player_Interaction playerInteraction = other.GetComponent<Player_Interaction>();

        if (playerInteraction == null)
            return;

        playerInteraction.GetInteractables().Remove(this);
        playerInteraction.UpdateClosestInteractable();
    }
}
