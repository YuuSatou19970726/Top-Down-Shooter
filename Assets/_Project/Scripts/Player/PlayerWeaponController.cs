using UnityEngine;

namespace TopDownShooter
{
    public class PlayerWeaponController : CustomMonoBehaviour
    {
        protected Player player;

        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float bulletSpeed;
        [SerializeField] private Transform gunPoint;

        [SerializeField] private Transform weaponHolder;

        protected override void LoadComponents()
        {
            if (this.player != null) return;
            this.player = GetComponent<Player>();
        }

        protected override void Start()
        {
            this.player.controls.Character.Fire.performed += context => this.Shoot();
        }

        private void Shoot()
        {
            GameObject newBullet =
                Instantiate(this.bulletPrefab, this.gunPoint.position, Quaternion.LookRotation(this.gunPoint.forward));

            newBullet.GetComponent<Rigidbody>().linearVelocity = this.BulletDirection() * this.bulletSpeed;
            Destroy(newBullet, 10);

            GetComponentInChildren<Animator>().SetTrigger(AnimationTags.TRIGGER_FIRE);
        }

        public Vector3 BulletDirection()
        {
            Transform aim = this.player.aim.Aim();

            Vector3 direction = (aim.position - this.gunPoint.position).normalized;

            if (this.player.aim.CanAimPrecisly() == false && this.player.aim.Target() == null)
                direction.y = 0;

            // this.weaponHolder.LookAt(this.aim);
            // this.gunPoint.LookAt(this.aim); TODO: find a better place for it.

            return direction;
        }

        public Transform GunPoint() => this.gunPoint;

        // void OnDrawGizmos()
        // {
        //     Gizmos.DrawLine(this.weaponHolder.position, this.weaponHolder.position + this.weaponHolder.forward * 25);
        //     Gizmos.color = Color.yellow;

        //     if (this.player != null)
        //         Gizmos.DrawLine(this.gunPoint.position, this.gunPoint.position + this.BulletDirection() * 25);
        // }
    }
}