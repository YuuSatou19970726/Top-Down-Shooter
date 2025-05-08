using UnityEngine;
namespace TopDownShooter
{
    public class Bullet : MonoBehaviour
    {
        private BoxCollider boxCollider;
        private new Rigidbody rigidbody;
        private MeshRenderer meshRenderer;
        private TrailRenderer trailRenderer;

        [SerializeField] GameObject bulletImpactFX;

        private Vector3 startPosition;
        private float flyDistance;
        private bool bulletDisabled;

        private void Awake()
        {
            this.boxCollider = GetComponent<BoxCollider>();
            this.rigidbody = GetComponent<Rigidbody>();
            this.meshRenderer = GetComponent<MeshRenderer>();
            this.trailRenderer = GetComponent<TrailRenderer>();
        }

        private void Update()
        {
            this.FadeTrail();
            this.DisableBullet();
            this.CheckReturnBulletToPool();
        }

        private void FadeTrail()
        {
            if (Vector3.Distance(this.startPosition, transform.position) > this.flyDistance - 1.5f)
                this.trailRenderer.time -= 2 * Time.deltaTime;
        }

        private void DisableBullet()
        {
            if (Vector3.Distance(this.startPosition, transform.position) > this.flyDistance && !this.bulletDisabled)
            {
                this.boxCollider.enabled = false;
                this.meshRenderer.enabled = false;
                this.bulletDisabled = true;
            }
        }

        private void CheckReturnBulletToPool()
        {
            if (this.trailRenderer.time < 0)
                this.ReturnBulletToPool();
        }

        void OnCollisionEnter(Collision collision)
        {
            // this.rb.constraints = RigidbodyConstraints.FreezeAll;
            CreateImpactFX(collision);

            this.ReturnBulletToPool();
        }

        private void ReturnBulletToPool() => ObjectPool.Instance.ReturnObject(gameObject);

        private void CreateImpactFX(Collision collision)
        {
            if (collision.contacts.Length > 0)
            {
                ContactPoint contact = collision.GetContact(0);

                GameObject newImpactFX = ObjectPool.Instance.GetObject(this.bulletImpactFX);
                // Instantiate(this.bulletImpactFX, contact.point, Quaternion.LookRotation(contact.normal));
                newImpactFX.transform.position = contact.point;
                newImpactFX.transform.rotation = Quaternion.LookRotation(contact.normal);

                ObjectPool.Instance.ReturnObject(newImpactFX, 1f);
            }
        }

        public void BulletSetup(float flyDistance)
        {
            this.bulletDisabled = false;
            this.boxCollider.enabled = true;
            this.meshRenderer.enabled = true;

            this.trailRenderer.time = .25f;
            this.startPosition = transform.position;
            // magic number .5f is a lenght of tip of the laser (check method UpdateAimVisuals() on PlayerAim script).
            this.flyDistance = flyDistance + .5f;
        }
    }
}