using UnityEngine;

namespace TopDownShooter
{
    public class PlayerAnimationEvents : CustomMonoBehaviour
    {
        private PlayerWeaponVisuals playerWeaponVisuals;
        private PlayerWeaponController playerWeaponController;

        public void SwitchOnWeaponModel() => this.playerWeaponVisuals.SwitchOnWeaponModel();

        protected override void LoadComponents()
        {
            this.LoadComponent();
        }

        private void LoadComponent()
        {
            this.playerWeaponVisuals = GetComponentInParent<PlayerWeaponVisuals>();
            this.playerWeaponController = GetComponentInParent<PlayerWeaponController>();
        }

        public void ReloadIsOver()
        {
            this.playerWeaponVisuals.MaximizeRigWeight();
            this.playerWeaponController.CurrentWeapon().RefillBullets();

            this.playerWeaponController.SetWeaponReady(true);
        }

        public void WeaponEquipingIsOver()
        {
            this.playerWeaponController.SetWeaponReady(true);
        }

        public void ReturnRig()
        {
            this.playerWeaponVisuals.MaximizeRigWeight();
            this.playerWeaponVisuals.MaximizeLeftHandWeight();
        }
    }
}