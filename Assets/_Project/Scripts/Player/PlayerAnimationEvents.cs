using UnityEngine;

namespace TopDownShooter
{
    public class PlayerAnimationEvents : CustomMonoBehaviour
    {
        private PlayerWeaponVisuals playerWeaponVisuals;

        protected override void LoadComponents()
        {
            this.LoadComponent();
        }

        private void LoadComponent()
        {
            this.playerWeaponVisuals = GetComponentInParent<PlayerWeaponVisuals>();
        }

        public void ReloadIsOver()
        {
            this.playerWeaponVisuals.MaximizeRigWeight();

            //refill-bullets
        }

        public void WeaponGrabIsOver()
        {
            this.ReturnRig();
            this.playerWeaponVisuals.SetBusyGrabbingWeaponTo(false);
        }

        public void ReturnRig()
        {
            this.playerWeaponVisuals.MaximizeRigWeight();
            this.playerWeaponVisuals.MaximizeLeftHandWeight();
        }
    }
}