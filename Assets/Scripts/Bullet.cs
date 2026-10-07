using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject bulletImpact;

    private Rigidbody rb=> GetComponent<Rigidbody>();

    private void OnCollisionEnter(Collision collision)
    {
        CreateImpactFx(collision);
        Destroy(gameObject);
    }

    private void CreateImpactFx(Collision collision)
    {
        if (collision.contacts.Length > 0)
        {
            ContactPoint contact = collision.contacts[0];
            GameObject newImpactFx = Instantiate(bulletImpact, contact.point, Quaternion.LookRotation(contact.normal));
            Destroy(newImpactFx, 1f);
        }
    }
}
