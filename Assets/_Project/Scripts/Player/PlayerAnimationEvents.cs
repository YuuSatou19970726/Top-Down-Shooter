using UnityEngine;

namespace TopDownShooter
{
    public class PlayerAnimationEvents : CustomMonoBehaviour
    {
        private WeaponVisualController weaponVisualController;

        protected override void LoadComponents()
        {
            this.LoadComponent();
        }

        private void LoadComponent()
        {
            this.weaponVisualController = GetComponentInParent<WeaponVisualController>();
        }

        public void ReloadIsOver()
        {
            this.weaponVisualController.ReturnRigWeigthToOne();

            //refill-bullets
        }

        public void WeaponGrabIsOver()
        {
            this.ReturnRig();
            this.weaponVisualController.SetBusyGrabbingWeaponTo(false);
        }

        public void ReturnRig()
        {
            this.weaponVisualController.ReturnRigWeigthToOne();
            this.weaponVisualController.ReturnWeigthToLeftHandIK();
        }
    }
}