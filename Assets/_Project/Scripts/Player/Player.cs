using UnityEngine;

namespace TopDownShooter
{
    public class Player : CustomMonoBehaviour
    {
        public PlayerController controls { get; private set; } // read-only
        public PlayerAim aim { get; private set; } // read-only
        public PlayerMovement movement { get; private set; } // read-only
        public PlayerWeaponController weapon { get; private set; } // read-only

        protected override void LoadComponents()
        {
            this.LoadComponent();
        }

        private void LoadComponent()
        {
            this.controls = new PlayerController();
            this.aim = GetComponent<PlayerAim>();
            this.movement = GetComponent<PlayerMovement>();
            this.weapon = GetComponent<PlayerWeaponController>();
        }

        protected override void OnEnable()
        {
            this.controls.Enable();
        }

        protected override void OnDisable()
        {
            this.controls.Disable();
        }
    }
}