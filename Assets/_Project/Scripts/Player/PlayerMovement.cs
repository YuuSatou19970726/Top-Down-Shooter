using UnityEditor.Rendering.LookDev;
using UnityEngine;

namespace TopDownShooter
{
    [RequireComponent(typeof(Player))]
    public class PlayerMovement : CustomMonoBehaviour
    {
        protected Player player;
        private PlayerController controls;
        protected CharacterController characterController;
        private Animator animator;

        [Header("Movement Info")]
        [SerializeField] protected float walkSpeed;
        [SerializeField] protected float runSpeed;
        [SerializeField] protected float turnSpeed;
        protected float speed;
        public Vector2 moveInput { get; private set; } // read-only
        protected Vector3 movementDirection;
        private float verticalVelocity;
        private bool isRunning;

        [SerializeField] private float gravityScale = 9.81f;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            this.speed = this.walkSpeed;
            this.AssignInputEvents();
        }

        protected override void Update()
        {
            this.Movement();
            this.ApplyRotation();
            this.AnimatorControllers();
        }

        protected override void LoadComponents()
        {
            this.LoadComponent();
        }

        protected virtual void LoadComponent()
        {
            if (this.player != null && this.characterController != null && this.animator != null) return;
            this.player = GetComponent<Player>();
            this.characterController = GetComponent<CharacterController>();
            this.animator = GetComponentInChildren<Animator>();
        }

        private void AssignInputEvents()
        {
            this.controls = this.player.controls;

            this.controls.Character.Movement.performed += context => this.moveInput = context.ReadValue<Vector2>();
            this.controls.Character.Movement.canceled += context => this.moveInput = Vector2.zero;

            this.controls.Character.Run.performed += context =>
            {
                this.speed = this.runSpeed;
                this.isRunning = true;
            };
            this.controls.Character.Run.canceled += context =>
            {
                this.speed = this.walkSpeed;
                this.isRunning = false;
            };
        }

        private void Movement()
        {
            if (this.characterController == null) return;
            this.movementDirection = new Vector3(this.moveInput.x, 0, this.moveInput.y);
            this.Gravity();

            if (this.movementDirection.magnitude > 0)
                this.characterController.Move(this.movementDirection * this.speed * Time.deltaTime);

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

        private void ApplyRotation()
        {
            Vector3 lookingDirection = this.player.aim.GetMouseHitInfo().point - transform.position;
            lookingDirection.y = 0f;
            lookingDirection.Normalize();

            // Smooth Rotation
            if (lookingDirection == Vector3.zero) return;
            Quaternion disiredRotation = Quaternion.LookRotation(lookingDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, disiredRotation, this.turnSpeed * Time.deltaTime);
        }

        private void AnimatorControllers()
        {
            float xVelocity = Vector3.Dot(this.movementDirection.normalized, transform.right);
            float zVelocity = Vector3.Dot(this.movementDirection.normalized, transform.forward);

            this.animator.SetFloat(AnimationTags.FLOAT_X_VELOCITY, xVelocity, 0.1f, Time.deltaTime);
            this.animator.SetFloat(AnimationTags.FLOAT_Z_VELOCITY, zVelocity, 0.1f, Time.deltaTime);

            bool playRunAnimation = this.isRunning && this.movementDirection.magnitude > 0;
            this.animator.SetBool(AnimationTags.BOOL_IS_RUNNING, playRunAnimation);
        }
    }
}