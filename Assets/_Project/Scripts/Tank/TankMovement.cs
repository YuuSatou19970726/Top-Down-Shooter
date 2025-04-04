using UnityEngine;

namespace TopDownShooter
{
    public class TankMovement : CustomMonoBehaviour
    {
        [Header("Movement Data")]
        public float moveSpeed;
        public float rotationSpeed;

        private Rigidbody rb;

        [Space]
        public LayerMask whatIsAimMask;
        public Transform aimTransform;

        protected override void LoadComponents()
        {
            this.rb = GetComponent<Rigidbody>();
        }

        protected override void Update()
        {
            this.UpdateAim();
        }

        protected override void FixedUpdate()
        {
            this.Movement();
        }

        protected void Movement()
        {
            Vector3 movement = transform.forward * this.moveSpeed * InputManager.Instance.verticalInput;
            this.rb.linearVelocity = movement;

            if (InputManager.Instance.verticalInput < 0)
                transform.Rotate(0, -InputManager.Instance.horizontalInput * this.rotationSpeed, 0);
            else
                transform.Rotate(0, InputManager.Instance.horizontalInput * this.rotationSpeed, 0);
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
    }
}