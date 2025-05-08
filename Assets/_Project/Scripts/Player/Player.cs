using UnityEngine;

namespace TopDownShooter
{
    public class Player : CustomMonoBehaviour
    {
        public PlayerController controls { get; private set; } // read-only
        public PlayerAim aim { get; private set; } // read-only
        public PlayerMovement movement { get; private set; } // read-only
        public PlayerWeaponController weapon { get; private set; } // read-only
        public PlayerWeaponVisuals weaponVisuals { get; private set; } // read-only
        public PlayerInteraction interaction { get; private set; } // read-only

        protected override void OnEnable()
        {
            this.controls.Enable();
        }

        protected override void LoadComponents()
        {
            this.LoadComponent();
        }

        private void LoadComponent()
        {
            if (this.controls != null && this.aim != null && this.movement != null && this.weapon != null && this.weaponVisuals != null
                && this.interaction != null)
                return;

            this.controls = new PlayerController();
            this.aim = GetComponent<PlayerAim>();
            this.movement = GetComponent<PlayerMovement>();
            this.weapon = GetComponent<PlayerWeaponController>();
            this.weaponVisuals = GetComponent<PlayerWeaponVisuals>();
            this.interaction = GetComponent<PlayerInteraction>();
        }

        protected override void OnDisable()
        {
            this.controls.Disable();
        }
    }
}