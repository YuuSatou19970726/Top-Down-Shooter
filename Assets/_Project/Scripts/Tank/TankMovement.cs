using UnityEngine;

namespace TopDownShooter
{
    public class TankMovement : CustomMonoBehaviour
    {
        private Rigidbody rb;

        [Header("Movement Data")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float rotationSpeed;

        [Header("Tower Data")]
        [SerializeField] private Transform towerTransform;
        [SerializeField] private float towerRotationSpeed;

        [Header("Aim Data")]
        [SerializeField] private LayerMask whatIsAimMask;
        [SerializeField] private Transform aimTransform;

        [Header("Gun Data")]
        [SerializeField] private Transform gunPointTransform;
        [SerializeField] private float bulletSpeed;
        [SerializeField] private GameObject bulletPrefab;

        protected override void LoadComponents()
        {
            this.rb = GetComponent<Rigidbody>();
        }

        protected override void Update()
        {
            this.UpdateAim();
            this.Shoot();
        }

        protected override void FixedUpdate()
        {
            this.Movement();
            this.BodyTankRotation();
            this.TowerTankRotation();
        }

        protected void Movement()
        {
            Vector3 movement = transform.forward * this.moveSpeed * InputManager.Instance.verticalInput;
            this.rb.linearVelocity = movement;
        }

        protected void BodyTankRotation()
        {
            if (InputManager.Instance.verticalInput < 0)
                transform.Rotate(0, -InputManager.Instance.horizontalInput * this.rotationSpeed, 0);
            else
                transform.Rotate(0, InputManager.Instance.horizontalInput * this.rotationSpeed, 0);
        }

        protected void TowerTankRotation()
        {
            // this.tankTower.LookAt(this.aimTransform);
            Vector3 direction = this.aimTransform.position - this.towerTransform.position;
            direction.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            this.towerTransform.rotation = Quaternion.RotateTowards(this.towerTransform.rotation, targetRotation, towerRotationSpeed);
        }

        protected void UpdateAim()
        {
            RaycastHit? hit = InputManager.Instance.GetAimTank(this.whatIsAimMask);

            if (hit.HasValue)
            {
                float fixedY = this.aimTransform.position.y;
                this.aimTransform.position = new Vector3(hit.Value.point.x, fixedY, hit.Value.point.z);
            }
        }

        protected void Shoot()
        {
            if (!InputManager.Instance.isClick) return;

            GameObject bullet = Instantiate(this.bulletPrefab, this.gunPointTransform.position, this.gunPointTransform.rotation);
            bullet.GetComponent<Rigidbody>().linearVelocity = this.gunPointTransform.forward * this.bulletSpeed;
            Destroy(bullet, 7);
        }
    }
}