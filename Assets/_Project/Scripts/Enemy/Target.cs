using UnityEngine;

namespace TopDownShooter
{
    public class Target : MonoBehaviour
    {
        [SerializeField] Material gotHitMaterial;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == TagTags.BULLET)
                GetComponent<MeshRenderer>().material = this.gotHitMaterial;
        }
    }
}