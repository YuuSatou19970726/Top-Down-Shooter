using UnityEditor.Rendering.LookDev;
using UnityEngine;

namespace TopDownShooter
{
    public class PlayerMovement : CustomMonoBehaviour
    {
        protected PlayerController controlls;
        protected CharacterController characterController;
        private Animator animator;

        [Header("Movement Info")]
        [SerializeField] protected float walkSpeed;
        protected Vector3 movementDirection;

        private float verticalVelocity;
        [SerializeField] private float gravityScale = 9.81f;

        [Header("Aim Info")]
        [SerializeField] private Transform aim;
        [SerializeField] private LayerMask aimPlayerMask;
        private Vector3 lookingDirection;

        protected Vector2 moveInput;
        protected Vector2 aimInput;

        protected override void Awake()
        {
            base.Awake();
            this.SetupInputController();
        }

        protected override void Update()
        {
            this.Movement();
            this.AimTowardsMouse();
            this.AnimatorControllers();
        }

        protected override void OnEnable()
        {
            this.controlls.Enable();
        }

        protected override void OnDisable()
        {
            this.controlls.Disable();
        }

        protected override void LoadComponents()
        {
            this.LoadComponent();
        }

        protected virtual void LoadComponent()
        {
            if (this.characterController != null && this.animator != null) return;
            this.characterController = GetComponent<CharacterController>();
            this.animator = GetComponentInChildren<Animator>();
        }

        private void SetupInputController()
        {
            this.controlls = new PlayerController();
            this.controlls.Character.Fire.performed += context => this.Shoot();

            this.controlls.Character.Movement.performed += context => this.moveInput = context.ReadValue<Vector2>();
            this.controlls.Character.Movement.canceled += context => this.moveInput = Vector2.zero;

            this.controlls.Character.Aim.performed += context => this.aimInput = context.ReadValue<Vector2>();
            this.controlls.Character.Aim.canceled += aimInput => this.aimInput = Vector2.zero;

        }

        private void Movement()
        {
            if (this.characterController == null) return;
            this.movementDirection = new Vector3(this.moveInput.x, 0, this.moveInput.y);
            this.Gravity();

            if (this.movementDirection.magnitude > 0)
                this.characterController.Move(this.movementDirection * this.walkSpeed * Time.deltaTime);

        }

        private void Gravity()
        {
            if (this.characterController.isGrounded == false)
            {
                this.verticalVelocity -= this.gravityScale * Time.deltaTime;
                this.movementDirection.y = this.verticalVelocity;
            }
            else
                this.verticalVelocity = -.5f;
        }

        private void AimTowardsMouse()
        {
            Ray ray = Camera.main.ScreenPointToRay(this.aimInput);

            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, this.aimPlayerMask))
            {
                this.lookingDirection = hit.point - transform.position;
                this.lookingDirection.y = 0f;
                this.lookingDirection.Normalize();

                transform.forward = this.lookingDirection;

                this.aim.position = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            }
        }

        private void AnimatorControllers()
        {
            float xVelocity = Vector3.Dot(this.movementDirection.normalized, transform.right);
            float zVelocity = Vector3.Dot(this.movementDirection.normalized, transform.forward);

            this.animator.SetFloat(AnimationTags.FLOAT_X_VELOCITY, xVelocity, 0.1f, Time.deltaTime);
            this.animator.SetFloat(AnimationTags.FLOAT_Z_VELOCITY, zVelocity, 0.1f, Time.deltaTime);
        }

        private void Shoot()
        {
            Debug.Log("SHOOT!");
        }
    }
}