using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] GameObject bulletImpactFX;
    private Rigidbody rb => GetComponent<Rigidbody>();

    void OnCollisionEnter(Collision collision)
    {
        // this.rb.constraints = RigidbodyConstraints.FreezeAll;
        CreateImpactFX(collision);

        Destroy(gameObject);
    }

    private void CreateImpactFX(Collision collision)
    {
        if (collision.contacts.Length > 0)
        {
            ContactPoint contact = collision.GetContact(0);

            GameObject newImpactFX =
                Instantiate(this.bulletImpactFX, contact.point, Quaternion.LookRotation(contact.normal));

            Destroy(newImpactFX, 1f);
        }
    }
}
