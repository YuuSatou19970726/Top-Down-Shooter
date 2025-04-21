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
        protected float speed;
        protected Vector3 movementDirection;

        private float verticalVelocity;
        private bool isRunning;
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
        }

        protected override void Start()
        {
            this.speed = this.walkSpeed;
            this.AssignInputEvents();
        }

        protected override void Update()
        {
            this.Movement();
            this.AimTowardsMouse();
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

            this.controls.Character.Aim.performed += context => this.aimInput = context.ReadValue<Vector2>();
            this.controls.Character.Aim.canceled += context => this.aimInput = Vector2.zero;

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

        private void AimTowardsMouse()
        {
            Ray ray = Camera.main.ScreenPointToRay(this.aimInput);

            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, this.aimPlayerMask))
            {
                this.lookingDirection = hit.point - transform.position;
                this.lookingDirection.y = 0f;
                this.lookingDirection.Normalize();

                transform.forward = this.lookingDirection;

                this.aim.position = new Vector3(hit.point.x, transform.position.y + 1, hit.point.z);
            }
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