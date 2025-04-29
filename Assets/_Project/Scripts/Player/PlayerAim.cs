using UnityEngine;
namespace TopDownShooter
{
    public class PlayerAim : CustomMonoBehaviour
    {
        protected Player player;
        private PlayerController controls;

        [Header("Aim Info")]
        [SerializeField] private Transform aim;
        [SerializeField] private LayerMask aimPlayerMask;
        private Vector2 aimInput;

        protected override void Start()
        {
            this.AssignInputEvents();
        }

        protected override void Update()
        {
            this.aim.position = new Vector3(this.GetMousePosition().x, transform.position.y + 1, this.GetMousePosition().z);
        }

        protected override void LoadComponents()
        {
            this.LoadComponent();
        }

        private void LoadComponent()
        {
            if (this.player != null) return;
            this.player = GetComponent<Player>();
        }

        public Vector3 GetMousePosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(this.aimInput);

            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, this.aimPlayerMask))
            {
                return hit.point;
            }
            return Vector3.zero;
        }

        private void AssignInputEvents()
        {
            this.controls = this.player.controls;

            this.controls.Character.Aim.performed += context => this.aimInput = context.ReadValue<Vector2>();
            this.controls.Character.Aim.canceled += context => this.aimInput = Vector2.zero;
        }
    }
}