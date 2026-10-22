using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected PlayerWeaponCotroller weaponCotroller;
    protected MeshRenderer mesh;


    [SerializeField] private Material highlightMaterial;
    protected Material defaultMaterial;

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
            weaponCotroller = other.GetComponent<PlayerWeaponCotroller>();
        }

        PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();

        if (playerInteraction == null)
            return;
       
        playerInteraction.GetInteractables().Add(this);
        playerInteraction.UpdateClosestInteractable();
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();

        if (playerInteraction == null)
            return;

        playerInteraction.GetInteractables().Remove(this);
        playerInteraction.UpdateClosestInteractable();
    }
}
