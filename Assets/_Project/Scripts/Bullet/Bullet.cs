using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb => GetComponent<Rigidbody>();

    void OnCollisionEnter(Collision collision)
    {
        this.rb.constraints = RigidbodyConstraints.FreezeAll;
    }
}
